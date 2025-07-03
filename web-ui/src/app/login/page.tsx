import LoginHeader from '@/components/auth/login-header';
import LoginForm from '@/components/auth/login-form';

export default function LoginPage() {
  return (
    <div className="min-h-screen bg-gray-50">
      <LoginHeader />
      <LoginForm />
    </div>
  );
}
