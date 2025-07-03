'use client';

import { useState } from 'react';
import SocialLoginButtons from './social-login-buttons';

export default function LoginForm() {
  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      // TODO: Implement actual login logic here
      console.log('Login attempt with email:', email);
      
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Redirect to dashboard or next page
      // router.push('/dashboard');
    } catch (error) {
      console.error('Login failed:', error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-[80vh] px-4">
      <div className="w-full max-w-md">
        <h1 className="text-2xl font-semibold mb-2 text-center text-gray-900">
          Sign in or create an account
        </h1>
        
        <p className="text-gray-600 text-center mb-8">
          You can sign in using your Booking.com account to access our services.
        </p>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
              Email address
            </label>
            <input
              id="email"
              type="email"
              placeholder="Enter your email address"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-400 text-gray-900"
            />
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full h-12 px-6 text-base font-medium text-white bg-[#0071c2] rounded-md hover:bg-[#003580] focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed transition-colors duration-200 flex items-center justify-center"
          >
            {loading ? (
              <>
                <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                  <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                  <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                </svg>
                Loading...
              </>
            ) : (
              'Continue with email'
            )}
          </button>
        </form>

        <div className="my-6 text-center text-gray-500">
          or use one of these options
        </div>

        <SocialLoginButtons />

        <div className="mt-8 text-xs text-center text-gray-500">
          By signing in or creating an account, you agree to our{' '}
          <a href="#" className="text-blue-600 hover:underline">
            Terms & conditions
          </a>{' '}
          and{' '}
          <a href="#" className="text-blue-600 hover:underline">
            Privacy statement
          </a>
        </div>

        <div className="mt-4 text-xs text-center text-gray-500">
          All rights reserved.<br />
          Copyright (2006 - 2025) - Booking.com™
        </div>
      </div>
    </div>
  );
}
