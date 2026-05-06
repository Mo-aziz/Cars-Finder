import React from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'

const HomePage = () => {
  const { isAuthenticated, user } = useAuth()

  return (
    <div className="container min-h-screen">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-12 items-center py-12">
        <div>
          <h1 className="text-5xl font-bold mb-4">Car Management System</h1>
          <p className="text-xl text-gray-400 mb-8">
            Manage your vehicles, engines, manufacturers, and profiles all in one place.
          </p>
          
          {isAuthenticated ? (
            <div className="space-y-4">
              <p className="text-lg">Welcome back, <span className="font-bold text-blue-400">{user?.username}</span>!</p>
              <div className="flex gap-4">
                <Link
                  to="/cars"
                  className="btn-primary inline-block"
                >
                  Browse Cars
                </Link>
                <Link
                  to="/manufacturers"
                  className="btn-secondary inline-block"
                >
                  View Manufacturers
                </Link>
              </div>
            </div>
          ) : (
            <div className="flex gap-4">
              <Link to="/login" className="btn-primary">
                Login
              </Link>
              <Link to="/signup" className="btn-secondary">
                Sign Up
              </Link>
            </div>
          )}
        </div>

        <div className="card">
          <h2 className="text-2xl font-bold mb-6">Features</h2>
          <ul className="space-y-3 text-gray-300">
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> Browse and manage cars
            </li>
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> Track manufacturers
            </li>
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> Manage car engines
            </li>
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> View car details and profiles
            </li>
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> Admin control panel
            </li>
            <li className="flex items-center">
              <span className="text-blue-400 mr-2">✓</span> Secure authentication
            </li>
          </ul>
        </div>
      </div>
    </div>
  )
}

export default HomePage
