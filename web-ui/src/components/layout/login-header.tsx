import Image from 'next/image';

export default function LoginHeader() {
  return (
    <header className="bg-[#003580] text-white py-4">
      <div className="container mx-auto px-4 flex justify-between items-center">
        <div className="text-xl font-bold">
          Booking.com
        </div>
        <div className="flex items-center gap-4">
          <button className="flex items-center gap-2 hover:bg-blue-700 px-2 py-1 rounded">
            <div className="w-5 h-4 bg-white rounded-sm flex items-center justify-center">
              <span className="text-xs text-blue-600 font-bold">GB</span>
            </div>
          </button>
          <button className="p-2 rounded-full hover:bg-blue-700">
            <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C13.1 2 14 2.9 14 4C14 5.1 13.1 6 12 6C10.9 6 10 5.1 10 4C10 2.9 10.9 2 12 2ZM21 9V7L15 1L9 7V9C9 10.1 9.9 11 11 11V16L7 20V22H17V20L13 16V11C14.1 11 15 10.1 15 9Z"/>
            </svg>
          </button>
        </div>
      </div>
    </header>
  );
}
