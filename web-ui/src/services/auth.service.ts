import { ApiResponse } from "../../shared/api";

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface LoginData {
  token: string;
  refreshToken: string;
  // Add other response fields as needed
}

export type LoginResponse = ApiResponse<LoginData>;

class AuthService {
  //private baseUrl = process.env.NEXT_PUBLIC_API_URL || '';
  private baseUrl = 'http://localhost:5051';

  async login(credentials: LoginRequest): Promise<LoginResponse> {
    try {
      const response = await fetch(`${this.baseUrl}/identity/authen/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
      });

      const result: LoginResponse = await response.json();
      return result;

    } catch (error) {
      console.error('Auth service error:', error);
      return {
        success: false,
        error: {
          code: 'NETWORK_ERROR',
            message: 'Failed to connect to the server. Please try again later.',
        },
      };
    }
}

  async logout(): Promise<void> {
    // TODO: Implement logout logic
    localStorage.removeItem('token');
  }

  async refreshToken(): Promise<string> {
    // TODO: Implement token refresh logic
    return '';
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

export const authService = new AuthService();
