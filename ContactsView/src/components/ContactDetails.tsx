import { useState, useEffect } from "react";
import { getContact, updateContact } from "../services/contactService";
import { getCategories } from "../services/categoryService";
import { getSubcategories } from "../services/subcategoryService";
import { useParams } from "react-router-dom";

export const ContactDetails = () => {
  const { id } = useParams();
  const [contact, setContact] = useState<IContact | undefined | null>(null);
  const [categories, setCategories] = useState<ICategory[] | undefined>([]);
  const [subcategories, setSubcategories] = useState<
    ISubcategory[] | undefined
  >([]);
  const [isEditing, setIsEditing] = useState(false);
  const [editedContact, setEditedContact] = useState<
    IContactBody | undefined | null
  >(null);

  useEffect(() => {
    if (id) {
      fetchContact(id);
      fetchCategories();
      fetchSubcategories();
    }
  }, [id]);

  const fetchContact = async (id: string) => {
    try {
      const response = await getContact(id);
      setContact(response);
      setEditedContact(response);
    } catch (err) {
      console.error("Failed to fetch contact", err);
      alert("Failed to fetch contact");
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await getCategories();
      setCategories(response);
    } catch (err) {
      console.error("Failed to fetch categories", err);
    }
  };

  const fetchSubcategories = async () => {
    try {
      const response = await getSubcategories();
      setSubcategories(response);
    } catch (err) {
      console.error("Failed to fetch subcategories", err);
    }
  };

  const handleUpdate = async () => {
    if (!contact) return null;
    if (editedContact) {
      try {
        await updateContact(contact.id, editedContact);
        setIsEditing(false);
        fetchContact(contact.id);
      } catch (err) {
        console.error("Failed to update contact", err);
        alert("Failed to update contact");
      }
    }
  };

  if (!contact) return <div>Loading...</div>;

  return (
    <div className="max-w-2xl mx-auto p-6">
      <a
        href="/contacts"
        className="mb-4 bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
      >
        Back to List
      </a>

      <div className="p-6 bg-white rounded-lg shadow">
        {isEditing && editedContact ? (
          <div>
            <input
              type="text"
              value={editedContact.firstName}
              onChange={(e) =>
                setEditedContact({
                  ...editedContact,
                  firstName: e.target.value,
                })
              }
              className="w-full p-2 mb-2 border rounded"
            />
            <input
              type="text"
              value={editedContact.lastName}
              onChange={(e) =>
                setEditedContact({ ...editedContact, lastName: e.target.value })
              }
              className="w-full p-2 mb-2 border rounded"
            />
            <input
              type="email"
              value={editedContact.email}
              onChange={(e) =>
                setEditedContact({ ...editedContact, email: e.target.value })
              }
              className="w-full p-2 mb-2 border rounded"
            />
            <input
              type="tel"
              value={editedContact.phone}
              onChange={(e) =>
                setEditedContact({ ...editedContact, phone: e.target.value })
              }
              className="w-full p-2 mb-2 border rounded"
            />
            <input
              type="date"
              value={editedContact.dateOfBirth}
              onChange={(e) =>
                setEditedContact({
                  ...editedContact,
                  dateOfBirth: e.target.value,
                })
              }
              className="w-full p-2 mb-2 border rounded"
            />
            <select
              value={editedContact.categoryId}
              onChange={(e) =>
                setEditedContact({
                  ...editedContact,
                  categoryId: e.target.value,
                  customSubcategory: "",
                })
              }
              className="w-full p-2 mb-2 border rounded"
            >
              <option value="">Select Category</option>
              {categories?.map((cat) => (
                <option key={cat.id} value={cat.id}>
                  {cat.name}
                </option>
              ))}
            </select>
            {editedContact.categoryId &&
              categories?.find((c) => c.id === editedContact.categoryId)
                ?.name === "sluzbowy" && (
                <select
                  value={editedContact.subcategoryId}
                  onChange={(e) =>
                    setEditedContact({
                      ...editedContact,
                      subcategoryId: e.target.value,
                    })
                  }
                  className="w-full p-2 mb-2 border rounded"
                >
                  <option value="">Select Subcategory</option>
                  {subcategories
                    ?.filter(
                      (sub) => sub.categoryId === editedContact.categoryId
                    )
                    ?.map((sub) => (
                      <option key={sub.id} value={sub.id}>
                        {sub.name}
                      </option>
                    ))}
                </select>
              )}
            {editedContact.categoryId &&
              categories?.find((c) => c.id === editedContact.categoryId)
                ?.name === "inny" && (
                <input
                  type="text"
                  placeholder="Custom Subcategory"
                  value={editedContact.subcategory?.name || ""}
                  onChange={(e) =>
                    setEditedContact({
                      ...editedContact,
                      customSubcategory: e.target.value,
                    })
                  }
                  className="w-full p-2 mb-2 border rounded"
                />
              )}
            <div className="flex justify-between">
              <button
                onClick={handleUpdate}
                className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
              >
                Save
              </button>
              <button
                onClick={() => setIsEditing(false)}
                className="bg-gray-600 text-white px-4 py-2 rounded hover:bg-gray-700"
              >
                Cancel
              </button>
            </div>
          </div>
        ) : (
          <div>
            <h2 className="text-2xl font-bold mb-4">
              {contact.firstName} {contact.lastName}
            </h2>
            <p>Email: {contact.email}</p>
            <p>Phone: {contact.phone}</p>
            <p>Date of Birth: {contact.dateOfBirth}</p>
            <p>Category: {contact.category?.name}</p>
            <p>Subcategory: {contact.subcategory?.name || "Empty"}</p>
            <button
              onClick={() => setIsEditing(true)}
              className="mt-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
            >
              Edit
            </button>
          </div>
        )}
      </div>
    </div>
  );
};
