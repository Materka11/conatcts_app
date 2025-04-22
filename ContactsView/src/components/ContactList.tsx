import { useEffect, useState } from "react";
import axios from "axios";

interface IProps {
  token: string;
  userId: string;
  refreshToken: string;
  setToken: (token: string) => void;
}

export const ContactList = ({
  token,
  userId,
  refreshToken,
  setToken,
}: IProps) => {
  const [contacts, setContacts] = useState<IContact[]>([]);
  const [categories, setCategories] = useState<ICategory[]>([]);
  const [subcategories, setSubcategories] = useState<ISubcategory[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [newContact, setNewContact] = useState<IContactBody>({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    dateOfBirth: "",
    categoryId: "",
    subcategoryId: "",
    customSubcategory: "",
  });

  useEffect(() => {
    fetchContacts();
    fetchCategories();
    fetchSubcategories();
  }, []);

  const fetchContacts = async () => {
    try {
      const response = await axios.get("/api/contacts", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setContacts(response.data);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (err.response?.status === 401) {
          //proba odswiezenia tokena
          try {
            const response = await axios.post("/api/auth/refresh", {
              userId: userId,
              refreshToken: refreshToken,
            });
            localStorage.setItem("token", response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);
            setToken(response.data.accessToken);
            fetchContacts();
          } catch (refreshErr) {
            console.error("Failed to refresh token", refreshErr);
            localStorage.removeItem("token");
            localStorage.removeItem("userId");
            localStorage.removeItem("refreshToken");
            window.location.reload();
          }
        }
      } else {
        console.error("Failed to fetch contacts", err);
      }
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await axios.get("/api/categories", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setCategories(response.data);
    } catch (err) {
      console.error("Failed to fetch categories", err);
    }
  };

  const fetchSubcategories = async () => {
    try {
      const response = await axios.get("/api/subcategories", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setSubcategories(response.data);
    } catch (err) {
      console.error("Failed to fetch subcategories", err);
    }
  };

  const handleCreateContact = async () => {
    try {
      await axios.post("/api/contacts", newContact, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setShowForm(false);
      setNewContact({
        firstName: "",
        lastName: "",
        email: "",
        phone: "",
        dateOfBirth: "",
        categoryId: "",
        subcategoryId: "",
        customSubcategory: "",
      });
      fetchContacts();
    } catch (err) {
      console.error("Failed to create contact", err);
      alert("Failed to create contact");
    }
  };

  const handleDelete = async (id: string) => {
    try {
      await axios.delete(`/api/contacts/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      fetchContacts();
    } catch (err) {
      console.error("Failed to delete contact", err);
      alert("Failed to delete contact");
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-6">
      <button
        onClick={() => setShowForm(!showForm)}
        className="mb-4 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
      >
        {showForm ? "Cancel" : "Add Contact"}
      </button>

      {showForm && (
        <div className="mb-6 p-4 bg-white rounded-lg shadow">
          <input
            type="text"
            placeholder="First Name"
            value={newContact.firstName}
            onChange={(e) =>
              setNewContact({ ...newContact, firstName: e.target.value })
            }
            className="w-full p-2 mb-2 border rounded"
          />
          <input
            type="text"
            placeholder="Last Name"
            value={newContact.lastName}
            onChange={(e) =>
              setNewContact({ ...newContact, lastName: e.target.value })
            }
            className="w-full p-2 mb-2 border rounded"
          />
          <input
            type="email"
            placeholder="Email"
            value={newContact.email}
            onChange={(e) =>
              setNewContact({ ...newContact, email: e.target.value })
            }
            className="w-full p-2 mb-2 border rounded"
          />
          <input
            type="tel"
            placeholder="Phone"
            value={newContact.phone}
            onChange={(e) =>
              setNewContact({ ...newContact, phone: e.target.value })
            }
            className="w-full p-2 mb-2 border rounded"
          />
          <input
            type="date"
            placeholder="Date of Birth"
            value={newContact.dateOfBirth}
            onChange={(e) =>
              setNewContact({ ...newContact, dateOfBirth: e.target.value })
            }
            className="w-full p-2 mb-2 border rounded"
          />
          <select
            value={newContact.categoryId}
            onChange={(e) =>
              setNewContact({
                ...newContact,
                categoryId: e.target.value,
                subcategoryId: "",
                customSubcategory: "",
              })
            }
            className="w-full p-2 mb-2 border rounded"
          >
            <option value="">Select Category</option>
            {categories.map((cat) => (
              <option key={cat.id} value={cat.id}>
                {cat.name}
              </option>
            ))}
          </select>
          {newContact.categoryId &&
            categories.find((c) => c.id === newContact.categoryId)?.name ===
              "Służbowy" && (
              <select
                value={newContact.subcategoryId}
                onChange={(e) =>
                  setNewContact({
                    ...newContact,
                    subcategoryId: e.target.value,
                  })
                }
                className="w-full p-2 mb-2 border rounded"
              >
                <option value="">Select Subcategory</option>
                {subcategories
                  .filter((sub) => sub.categoryId === newContact.categoryId)
                  .map((sub) => (
                    <option key={sub.id} value={sub.id}>
                      {sub.name}
                    </option>
                  ))}
              </select>
            )}
          {newContact.categoryId &&
            categories.find((c) => c.id === newContact.categoryId)?.name ===
              "Inny" && (
              <input
                type="text"
                placeholder="Custom Subcategory"
                value={newContact.customSubcategory}
                onChange={(e) =>
                  setNewContact({
                    ...newContact,
                    customSubcategory: e.target.value,
                  })
                }
                className="w-full p-2 mb-2 border rounded"
              />
            )}
          <button
            onClick={handleCreateContact}
            className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
          >
            Create Contact
          </button>
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {contacts.map((contact) => (
          <div key={contact.id} className="p-4 bg-white rounded-lg shadow">
            <h3 className="text-lg font-bold">
              {contact.firstName} {contact.lastName}
            </h3>
            <p>Email: {contact.email}</p>
            <p>Category: {contact.category?.name}</p>
            <p>Subcategory: {contact.subcategory?.name}</p>
            <div className="mt-2">
              <button
                onClick={() => {
                  localStorage.setItem("selectedContactId", contact.id);
                  window.location.href = "#contactDetails";
                }}
                className="mr-2 bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700"
              >
                Details
              </button>
              <button
                onClick={() => handleDelete(contact.id)}
                className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};
