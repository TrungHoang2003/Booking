import Link from 'next/link';

export default function LoginHeader() {
  return (
    <header className="bg-[#003580] text-white">
      <div className="container mx-auto px-4 py-4">
        <div className="flex justify-between items-center">
          <Link href="/" className="text-xl font-bold hover:text-blue-200 transition-colors">
            Booking.com
          </Link>
          
          <div className="flex items-center gap-4">
            {/* Language Selector */}
            <button className="flex items-center gap-2 p-2 rounded-md hover:bg-blue-700 transition-colors">
              <svg width="20" height="15" viewBox="0 0 20 15" className="rounded-sm">
                <rect width="20" height="15" fill="#012169"/>
                <path d="M0 0l20 15M20 0L0 15" stroke="#fff" strokeWidth="2"/>
                <path d="M0 0l20 15M20 0L0 15" stroke="#C8102E" strokeWidth="1"/>
                <path d="M10 0v15M0 7.5h20" stroke="#fff" strokeWidth="3"/>
                <path d="M10 0v15M0 7.5h20" stroke="#C8102E" strokeWidth="1"/>
              </svg>
            </button>

            {/* Help/Support Button */}
            <button className="p-2 rounded-full hover:bg-blue-700 transition-colors">
              <svg width="20" height="20" viewBox="0 0 24 24" fill="currentColor">
                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.45 12.9 13 13.5 13 15h-2v-.5c0-1.1.45-2.1 1.17-2.83l1.24-1.26c.37-.36.59-.86.59-1.41 0-1.1-.9-2-2-2s-2 .9-2 2H8c0-2.21 1.79-4 4-4s4 1.79 4 4c0 .88-.36 1.68-.93 2.25z"/>
              </svg>
            </button>
          </div>
        </div>
      </div>
    </header>
  );
}
