import { Metadata } from 'next';

export const metadata: Metadata = {
  title: 'Sign in or create an account - Booking.com',
  description: 'Sign in to your Booking.com account to access your bookings and manage your reservations.',
  keywords: ['booking', 'login', 'sign in', 'account', 'travel'],
};

export default function LoginLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return children;
}
