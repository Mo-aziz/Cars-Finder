import axios from 'axios'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api'

const api = axios.create({
  baseURL: API_URL,
  withCredentials: true,
  timeout: 10000,
})

// HTTP-only cookies are automatically sent with withCredentials: true


// Pass errors through without modification
api.interceptors.response.use(
  (response) => response,
  (error) => Promise.reject(error)
)

export default api
