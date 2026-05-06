import api from './api'

export const authService = {
  login: async (username, password) => {
    try {
      const response = await api.post('/auth/login', { username, password })
      if (response.data.username) {
        const user = {
          username: response.data.username,
          role: response.data.role
        }
        localStorage.setItem('user', JSON.stringify(user))
      }
      return response.data
    } catch (error) {
      const errorData = error.response?.data
      
      if (error.response?.status === 401) {
        throw new Error('Invalid username or password')
      } else if (error.response?.status === 400) {
        throw new Error(errorData?.message || 'Invalid username or password')
      } else if (error.response?.status === 404) {
        throw new Error('Username not found')
      }
      
      throw new Error(errorData?.message || 'Login failed. Please try again.')
    }
  },

  signup: async (username, password, role) => {
    try {
      const response = await api.post('/auth/signup', { username, password, role })
      return response.data
    } catch (error) {
      const errorData = error.response?.data
      
      if (error.response?.status === 400) {
        throw new Error(errorData?.message || 'Invalid input. Please check your details.')
      } else if (error.response?.status === 409) {
        throw new Error('Username already exists. Please choose another one.')
      }
      
      throw new Error(errorData?.message || 'Signup failed. Please try again.')
    }
  },

  logout: () => {
    localStorage.removeItem('user')
    // JWT token is stored in HTTP-only cookie, no need to manually clear it
    // Browser will handle the cookie deletion via server response
  },

  refreshToken: async () => {
    try {
      const response = await api.post('/auth/refresh-token')
      return response.data
    } catch (error) {
      throw new Error(error.response?.data?.message || 'Token refresh failed')
    }
  },

  getCurrentUser: () => {
    const user = localStorage.getItem('user')
    return user ? JSON.parse(user) : null
  }
}
