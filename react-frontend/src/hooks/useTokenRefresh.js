import { useEffect } from 'react'
import { useNavigate, useLocation } from 'react-router-dom'
import api from '../services/api'

export const useTokenRefresh = () => {
  const navigate = useNavigate()
  const location = useLocation()

  useEffect(() => {
    // Add response interceptor for handling 401s on protected routes
    const interceptor = api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          // Don't redirect if we're on login/signup pages
          const isAuthPage = location.pathname === '/login' || location.pathname === '/signup'
          
          if (!isAuthPage) {
            // Redirect to login on 401 from protected routes
            // JWT token is in HTTP-only cookie, browser will handle cleanup
            localStorage.removeItem('user')
            navigate('/login', { replace: true })
          }
        }
        
        return Promise.reject(error)
      }
    )

    return () => {
      api.interceptors.response.eject(interceptor)
    }
  }, [navigate, location.pathname])
}

