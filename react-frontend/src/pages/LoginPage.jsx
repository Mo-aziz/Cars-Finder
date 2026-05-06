import React, { useState, useEffect, useRef } from 'react'
import { Link, Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'

const LoginPage = () => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)
  const { login, isAuthenticated } = useAuth()
  const isMountedRef = useRef(true)

  useEffect(() => {
    isMountedRef.current = true
    return () => {
      isMountedRef.current = false
    }
  }, [])

  // Restore error from localStorage on mount
  useEffect(() => {
    const storedError = localStorage.getItem('loginError')
    if (storedError) {
      if (isMountedRef.current) setError(storedError)
      localStorage.removeItem('loginError')
    }
  }, [])

  if (isAuthenticated) {
    return <Navigate to="/" replace />
  }

  const handleSubmit = async (e) => {
    e?.preventDefault?.()
    
    if (!username.trim() || !password.trim()) {
      if (isMountedRef.current) setError('Please enter both username and password')
      return
    }

    if (isMountedRef.current) setError('')
    if (isMountedRef.current) setLoading(true)

    try {
      await login(username, password)
    } catch (err) {
      const errorMessage = err.message || 'Login failed'
      
      let displayError = 'Login failed. Please try again.'
      if (errorMessage.includes('Invalid') || errorMessage.includes('Unauthorized')) {
        displayError = 'Invalid username or password'
      } else if (errorMessage.includes('not found')) {
        displayError = 'Username does not exist'
      } else if (errorMessage.includes('Network') || errorMessage.includes('timeout')) {
        displayError = 'Network error. Please check your connection.'
      } else {
        displayError = errorMessage
      }
      
      if (isMountedRef.current) {
        setError(displayError)
        setUsername('')
        setPassword('')
      } else {
        localStorage.setItem('loginError', displayError)
      }
    } finally {
      if (isMountedRef.current) setLoading(false)
    }
  }

  return (
    <div className="container flex justify-center items-center min-h-screen">
      <div className="card w-full max-w-md">
        <h1 className="text-3xl font-bold mb-6 text-center">Login</h1>

        {error && <Alert message={error} type="error" onClose={() => setError('')} />}

        <div className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Username</label>
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Password</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="input-field"
              required
            />
          </div>

          <button
            type="button"
            onClick={handleSubmit}
            disabled={loading}
            className="w-full btn-primary disabled:opacity-50"
          >
            {loading ? 'Logging in...' : 'Login'}
          </button>
        </div>

        <p className="text-center mt-4 text-gray-400">
          Don't have an account?{' '}
          <Link to="/signup" className="text-blue-400 hover:text-blue-300">
            Sign up here
          </Link>
        </p>
      </div>
    </div>
  )
}

export default LoginPage
