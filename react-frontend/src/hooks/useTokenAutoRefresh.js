import { useEffect } from 'react'
import api from '../services/api'

export const useTokenAutoRefresh = (isAuthenticated) => {
  useEffect(() => {
    if (!isAuthenticated) return

    // Refresh token every 55 minutes (before 60-minute expiration)
    const refreshInterval = 55 * 60 * 1000

    const refreshToken = async () => {
      try {
        await api.post('/auth/refresh-token')
      } catch (error) {
        console.error('Token refresh failed:', error)
        // Token refresh failed, user will be redirected on next 401
      }
    }

    // Set up interval for token refresh
    const intervalId = setInterval(refreshToken, refreshInterval)

    // Also refresh on page visibility change (when user returns to tab)
    const handleVisibilityChange = () => {
      if (!document.hidden) {
        refreshToken()
      }
    }

    document.addEventListener('visibilitychange', handleVisibilityChange)

    return () => {
      clearInterval(intervalId)
      document.removeEventListener('visibilitychange', handleVisibilityChange)
    }
  }, [isAuthenticated])
}
