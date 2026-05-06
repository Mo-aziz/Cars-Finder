import React from 'react'
import { useAuth } from '../context/AuthContext'
import { Link } from 'react-router-dom'

const Navigation = () => {
  const { user, logout, isAuthenticated } = useAuth()
  const displayRole = user?.role === 'Instructor' ? 'Employee' : user?.role

  const handleSwaggerClick = () => {
    window.open('http://localhost:5000/swagger', '_blank')
  }

  return (
    <nav className="bg-gray-800 text-white shadow-lg">
      <div className="max-w-6xl mx-auto px-4">
        <div className="flex justify-between items-center py-4">
          <Link to="/" className="text-2xl font-bold text-blue-400">
            Carbank
          </Link>
          
          <button
            onClick={handleSwaggerClick}
            className="text-sm bg-yellow-600 hover:bg-yellow-700 text-white px-3 py-1 rounded transition"
            title="View API Documentation"
          >
            API Docs
          </button>
          
          <div className="flex gap-6 items-center">
            {isAuthenticated ? (
              <>
                <Link to="/" className="hover:text-blue-400 transition">Home</Link>
                <Link to="/cars" className="hover:text-blue-400 transition">Cars</Link>
                <Link to="/manufacturers" className="hover:text-blue-400 transition">Manufacturers</Link>
                <Link to="/engines" className="hover:text-blue-400 transition">Engines</Link>
                <Link to="/car-profiles" className="hover:text-blue-400 transition">Car Profiles</Link>
                <Link to="/car-engines" className="hover:text-blue-400 transition">Car Engines</Link>
                
                <div className="flex items-center gap-4">
                  <span className="text-sm">Welcome, {user?.username}</span>
                  {(displayRole === 'Admin' || displayRole === 'Employee') && (
                    <span className="text-xs bg-blue-600 px-2 py-1 rounded">{displayRole}</span>
                  )}
                  <button
                    onClick={logout}
                    className="btn-secondary text-sm"
                  >
                    Logout
                  </button>
                </div>
              </>
            ) : (
              <>
                <Link to="/login" className="hover:text-blue-400 transition">Login</Link>
                <Link to="/signup" className="btn-primary text-sm">Sign Up</Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  )
}

export default Navigation
