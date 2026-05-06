import api from './api'

export const carService = {
  getAll: async () => {
    try {
      const response = await api.get('/car')
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch cars' }
    }
  },

  getById: async (id) => {
    try {
      const response = await api.get(`/car/${id}`)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch car' }
    }
  },

  create: async (carData) => {
    try {
      const response = await api.post('/car', carData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to create car' }
    }
  },

  update: async (id, carData) => {
    try {
      const response = await api.put(`/car/${id}`, carData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to update car' }
    }
  },

  delete: async (id) => {
    try {
      await api.delete(`/car/${id}`)
      return { message: 'Car deleted successfully' }
    } catch (error) {
      throw error.response?.data || { message: 'Failed to delete car' }
    }
  }
}
