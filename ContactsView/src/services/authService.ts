import { client } from "./middleware";
import { AUTH_URL } from "./endpoints";

export const RegisterService = async (email: string, password: string) => {
  try {
    const response = await client.post<IUser>(`${AUTH_URL}/register`, {
      email,
      password,
    });

    if (response.data) {
      return response.data;
    }
  } catch (err) {
    console.error(err);
  }
};

export const LoginService = async (email: string, password: string) => {
  try {
    const response = await client.post<ILoginResponse>(`${AUTH_URL}/login`, {
      email,
      password,
    });

    if (response.data) {
      localStorage.setItem("token", response.data.accessToken);
      localStorage.setItem("refreshToken", response.data.refreshToken);
      localStorage.setItem("userId", response.data.userId);

      return response.data;
    }
  } catch (err) {
    console.error(err);
  }
};

export const RefreshToken = async () => {
  try {
    const refreshToken = localStorage.getItem("refreshToken");
    const userId = localStorage.getItem("userId");

    if (refreshToken && userId) {
      const response = await client.post<ILoginResponse>(
        `${AUTH_URL}/refresh-token`,
        {
          refreshToken,
          userId,
        }
      );

      if (response.data) {
        const { accessToken, refreshToken: newRefreshToken } = response.data;

        localStorage.setItem("token", accessToken);
        localStorage.setItem("refreshToken", newRefreshToken);

        return response.data;
      }
    }
  } catch (err) {
    console.error(err);
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("refreshToken");
    window.location.reload();
  }
};
