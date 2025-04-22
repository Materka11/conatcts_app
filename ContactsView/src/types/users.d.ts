interface IUser {
  id: string;
  email: string;
  passwordHash: string;
  refreshToken: string;
  refreshTokenExpiryTime: string;
}

interface ILoginResponse {
  accessToken: string;
  refreshToken: string;
  userId: string;
}
