import React, { useState, useEffect, useRef } from 'react'
import { useNavigate, Link, Navigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'

const SignupPage = () => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [confirmPassword, setConfirmPassword] = useState('')
  const [role, setRole] = useState('User')
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const { signup, isAuthenticated } = useAuth()
  const isMountedRef = useRef(true)

  useEffect(() => {
    isMountedRef.current = true
    return () => {
      isMountedRef.current = false
    }
  }, [])

  // Restore error or success from localStorage on mount
  useEffect(() => {
    const storedError = localStorage.getItem('signupError')
    if (storedError) {
      if (isMountedRef.current) setError(storedError)
      localStorage.removeItem('signupError')
    }

    const storedSuccess = localStorage.getItem('signupSuccess')
    if (storedSuccess) {
      if (isMountedRef.current) setSuccess(storedSuccess)
      localStorage.removeItem('signupSuccess')
      setTimeout(() => {
        navigate('/login', { replace: true })
      }, 2000)
    }
  }, [])

  if (isAuthenticated) {
    return <Navigate to="/" replace />
  }

  const handleSubmit = async () => {
    if (isMountedRef.current) setError('')

    if (!username.trim() || !password.trim() || !confirmPassword.trim()) {
      if (isMountedRef.current) setError('Please fill in all fields')
      return
    }

    if (password !== confirmPassword) {
      if (isMountedRef.current) setError('Passwords do not match')
      return
    }

    if (isMountedRef.current) setLoading(true)

    try {
      await signup(username, password, role)
      const successMsg = 'Signup successful! Redirecting to login...'
      if (isMountedRef.current) {
        setError('')
        setSuccess(successMsg)
        setTimeout(() => {
          navigate('/login', { replace: true })
        }, 2000)
      } else {
        localStorage.setItem('signupSuccess', successMsg)
      }
    } catch (err) {
      const errorMessage = err.message || 'Signup failed'
      
      let displayError = 'Signup failed. Please try again.'
      if (errorMessage.includes('already')) {
        displayError = 'Username already exists. Please choose another one.'
      } else if (errorMessage.includes('invalid')) {
        displayError = 'Invalid username or password format'
      } else {
        displayError = errorMessage
      }
      
      if (isMountedRef.current) {
        setError(displayError)
        setUsername('')
        setPassword('')
        setConfirmPassword('')
      } else {
        localStorage.setItem('signupError', displayError)
      }
    } finally {
      if (isMountedRef.current) setLoading(false)
    }
  }

  return (
    <div className="container flex justify-center items-center min-h-screen">
      <div className="card w-full max-w-md">
        <h1 className="text-3xl font-bold mb-6 text-center">Sign Up</h1>

        {success ? (
          <Alert message={success} type="success" />
        ) : (
          <>
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

              <div>
                <label className="block text-sm font-medium mb-2">Confirm Password</label>
                <input
                  type="password"
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className="input-field"
                  required
                />
              </div>

              <div>
                <label className="block text-sm font-medium mb-2">Role</label>
                <select
                  value={role}
                  onChange={(e) => setRole(e.target.value)}
                  className="input-field"
                >
                  <option value="User">User</option>
                  <option value="Employee">Employee</option>
                  <option value="Admin">Admin</option>
                </select>
              </div>

              <button
                type="button"
                onClick={handleSubmit}
                disabled={loading}
                className="w-full btn-primary disabled:opacity-50"
              >
                {loading ? 'Signing up...' : 'Sign Up'}
              </button>
            </div>

            <p className="text-center mt-4 text-gray-400">
              Already have an account?{' '}
              <Link to="/login" className="text-blue-400 hover:text-blue-300">
                Login here
              </Link>
            </p>
          </>
        )}
      </div>
    </div>
  )
}

export default SignupPage
