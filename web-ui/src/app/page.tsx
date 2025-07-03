import Link from "next/link";

export default function Home() {
  return (
    <div className="min-h-screen bg-white">
      {/* Header */}
      <header className="bg-[#003580] text-white">
        <div className="max-w-7xl mx-auto px-4 py-3">
          <div className="flex justify-between items-center">
            <div className="flex items-center gap-6">
              <div className="text-xl font-bold">
                Booking.com
              </div>
              
              <nav className="hidden md:flex items-center gap-4 text-sm">
                <a href="#" className="hover:text-blue-200 transition-colors">Stays</a>
                <a href="#" className="hover:text-blue-200 transition-colors">Flights</a>
                <a href="#" className="hover:text-blue-200 transition-colors">Car rentals</a>
                <a href="#" className="hover:text-blue-200 transition-colors">Attractions</a>
                <a href="#" className="hover:text-blue-200 transition-colors">Airport taxis</a>
              </nav>
            </div>
            
            <div className="flex items-center gap-4">
              <button className="text-sm hover:text-blue-200 transition-colors">VND</button>
              <button className="text-sm hover:text-blue-200 transition-colors">🇻🇳</button>
              <button className="text-sm hover:text-blue-200 transition-colors">❓</button>
              <button className="text-sm hover:text-blue-200 transition-colors">List your property</button>
              <Link 
                href="/login"
                className="px-4 py-2 border border-white rounded-md hover:bg-white hover:text-[#003580] transition-colors text-sm"
              >
                Register
              </Link>
              <Link 
                href="/login"
                className="px-4 py-2 bg-white text-[#003580] rounded-md hover:bg-gray-100 transition-colors font-medium text-sm"
              >
                Sign in
              </Link>
            </div>
          </div>
        </div>
      </header>

      {/* Hero Section with Car Rental */}
      <section className="relative bg-gradient-to-r from-green-900 to-green-700 text-white py-16">
        <div className="max-w-7xl mx-auto px-4">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 items-center">
            <div>
              <h1 className="text-4xl font-bold mb-4">
                Enjoy 10% discounts on<br />
                select car rentals
              </h1>
              <p className="text-lg mb-6">
                Book car rentals online and save money
              </p>
              <button className="bg-[#0071c2] text-white px-6 py-3 rounded-md hover:bg-[#003580] transition-colors font-medium">
                Search car rental deals
              </button>
            </div>
            <div className="hidden lg:block">
              <div className="relative">
                <img 
                  src="/api/placeholder/600/400" 
                  alt="Car rental" 
                  className="rounded-lg shadow-lg"
                />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Main Search Section */}
      <section className="bg-[#003580] text-white py-8">
        <div className="max-w-7xl mx-auto px-4">
          <div className="bg-[#ffb000] rounded-lg p-6">
            <div className="grid grid-cols-1 md:grid-cols-5 gap-4">
              <div className="relative">
                <input
                  type="text"
                  placeholder="Where are you going?"
                  className="w-full px-4 py-3 rounded-md text-gray-900 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
                <div className="absolute left-3 top-3">
                  <svg className="w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                  </svg>
                </div>
              </div>
              
              <div>
                <input
                  type="date"
                  className="w-full px-4 py-3 rounded-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <input
                  type="date"
                  className="w-full px-4 py-3 rounded-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
              </div>
              
              <div>
                <select className="w-full px-4 py-3 rounded-md text-gray-900 focus:outline-none focus:ring-2 focus:ring-blue-500">
                  <option>2 adults · 0 children · 1 room</option>
                  <option>1 adult · 0 children · 1 room</option>
                  <option>2 adults · 1 child · 1 room</option>
                </select>
              </div>
              
              <div>
                <button className="w-full bg-[#0071c2] text-white py-3 px-4 rounded-md hover:bg-[#003580] transition-colors font-medium">
                  Search
                </button>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Special Offers */}
      <section className="py-12 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4">
          <div className="flex justify-between items-center mb-8">
            <h2 className="text-2xl font-bold text-gray-900">
              Still interested in these properties?
            </h2>
            <button className="text-[#0071c2] hover:underline">See all</button>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
            {[
              { title: "Viet An Hotel Camela Hotel", location: "Nha Trang", price: "VND 432,000", image: "/api/placeholder/300/200" },
              { title: "Danang Falcon Centre", location: "Da Nang", price: "VND 1,102,000", image: "/api/placeholder/300/200" },
              { title: "Flamingo Beach Sai Gon", location: "Ho Chi Minh City", price: "VND 498,000", image: "/api/placeholder/300/200" },
              { title: "A La Carte Danang Beach", location: "Da Nang", price: "VND 1,230,000", image: "/api/placeholder/300/200" }
            ].map((property, index) => (
              <div key={index} className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow">
                <img src={property.image} alt={property.title} className="w-full h-48 object-cover" />
                <div className="p-4">
                  <h3 className="font-semibold text-gray-900 mb-1">{property.title}</h3>
                  <p className="text-sm text-gray-600 mb-2">{property.location}</p>
                  <p className="text-lg font-bold text-gray-900">{property.price}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Offers Section */}
      <section className="py-12">
        <div className="max-w-7xl mx-auto px-4">
          <div className="flex justify-between items-center mb-8">
            <h2 className="text-2xl font-bold text-gray-900">Offers</h2>
            <p className="text-gray-600">Promotions, deals, and special offers for you</p>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="bg-blue-50 rounded-lg p-6">
              <h3 className="text-xl font-bold text-gray-900 mb-2">
                Quick escape, qualify time
              </h3>
              <p className="text-gray-600 mb-4">
                Book now and enjoy flexible cancellation options
              </p>
              <button className="bg-[#0071c2] text-white px-6 py-2 rounded-md hover:bg-[#003580] transition-colors">
                Find Gateway Deals
              </button>
            </div>
            
            <div className="bg-yellow-50 rounded-lg p-6">
              <h3 className="text-xl font-bold text-gray-900 mb-2">
                Find apartments & homes for longer stays
              </h3>
              <p className="text-gray-600 mb-4">
                Get the space you need for your next adventure
              </p>
              <button className="bg-[#0071c2] text-white px-6 py-2 rounded-md hover:bg-[#003580] transition-colors">
                Browse Properties
              </button>
            </div>
          </div>
        </div>
      </section>

      {/* Property Types */}
      <section className="py-12 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4">
          <h2 className="text-2xl font-bold text-gray-900 mb-8">Browse by property type</h2>
          
          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
            {[
              { name: "Hotels", count: "869,000 hotels", image: "/api/placeholder/200/150" },
              { name: "Apartments", count: "680,000 apartments", image: "/api/placeholder/200/150" },
              { name: "Resorts", count: "19,000 resorts", image: "/api/placeholder/200/150" },
              { name: "Villas", count: "480,000 villas", image: "/api/placeholder/200/150" }
            ].map((type, index) => (
              <div key={index} className="bg-white rounded-lg overflow-hidden hover:shadow-lg transition-shadow cursor-pointer">
                <img src={type.image} alt={type.name} className="w-full h-32 object-cover" />
                <div className="p-4">
                  <h3 className="font-semibold text-gray-900">{type.name}</h3>
                  <p className="text-sm text-gray-600">{type.count}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Popular Destinations */}
      <section className="py-12">
        <div className="max-w-7xl mx-auto px-4">
          <h2 className="text-2xl font-bold text-gray-900 mb-8">Trending destinations</h2>
          <p className="text-gray-600 mb-8">Most popular choices for travellers from Vietnam</p>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {[
              { name: "Ho Chi Minh City", image: "/api/placeholder/300/200", flag: "🇻🇳" },
              { name: "Da Nang", image: "/api/placeholder/300/200", flag: "🇻🇳" },
              { name: "Vung Tau", image: "/api/placeholder/300/200", flag: "🇻🇳" },
              { name: "Hanoi", image: "/api/placeholder/300/200", flag: "🇻🇳" },
              { name: "Da Lat", image: "/api/placeholder/300/200", flag: "🇻🇳" }
            ].map((destination, index) => (
              <div key={index} className="relative rounded-lg overflow-hidden hover:shadow-lg transition-shadow cursor-pointer">
                <img src={destination.image} alt={destination.name} className="w-full h-48 object-cover" />
                <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/60 to-transparent p-4">
                  <h3 className="text-white font-semibold text-lg">{destination.name} {destination.flag}</h3>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-[#003580] text-white py-12">
        <div className="max-w-7xl mx-auto px-4">
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
            <div>
              <h3 className="font-semibold mb-4">Support</h3>
              <ul className="space-y-2 text-sm">
                <li><a href="#" className="hover:text-blue-200">Coronavirus (COVID-19) FAQs</a></li>
                <li><a href="#" className="hover:text-blue-200">Manage your trips</a></li>
                <li><a href="#" className="hover:text-blue-200">Contact Customer Service</a></li>
              </ul>
            </div>
            
            <div>
              <h3 className="font-semibold mb-4">Discover</h3>
              <ul className="space-y-2 text-sm">
                <li><a href="#" className="hover:text-blue-200">Genius loyalty programme</a></li>
                <li><a href="#" className="hover:text-blue-200">Seasonal and holiday deals</a></li>
                <li><a href="#" className="hover:text-blue-200">Travel articles</a></li>
              </ul>
            </div>
            
            <div>
              <h3 className="font-semibold mb-4">Terms and settings</h3>
              <ul className="space-y-2 text-sm">
                <li><a href="#" className="hover:text-blue-200">Privacy & cookies</a></li>
                <li><a href="#" className="hover:text-blue-200">Terms & conditions</a></li>
                <li><a href="#" className="hover:text-blue-200">Partner help</a></li>
              </ul>
            </div>
            
            <div>
              <h3 className="font-semibold mb-4">Partners</h3>
              <ul className="space-y-2 text-sm">
                <li><a href="#" className="hover:text-blue-200">Extranet login</a></li>
                <li><a href="#" className="hover:text-blue-200">Partner help</a></li>
                <li><a href="#" className="hover:text-blue-200">List your property</a></li>
              </ul>
            </div>
          </div>
          
          <div className="border-t border-blue-700 mt-8 pt-8 text-center text-sm">
            <p>Copyright © 2006-2025 Booking.com™. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}
