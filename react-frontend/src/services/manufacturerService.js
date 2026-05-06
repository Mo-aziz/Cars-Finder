import api from './api'

export const manufacturerService = {
  getAll: async () => {
    try {
      const response = await api.get('/manufacturer')
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch manufacturers' }
    }
  },

  getById: async (id) => {
    try {
      const response = await api.get(`/manufacturer/${id}`)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch manufacturer' }
    }
  },

  create: async (manufacturerData) => {
    try {
      const response = await api.post('/manufacturer', manufacturerData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to create manufacturer' }
    }
  },

  update: async (id, manufacturerData) => {
    try {
      const response = await api.put(`/manufacturer/${id}`, manufacturerData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to update manufacturer' }
    }
  },

  delete: async (id) => {
    try {
      await api.delete(`/manufacturer/${id}`)
      return { message: 'Manufacturer deleted successfully' }
    } catch (error) {
      throw error.response?.data || { message: 'Failed to delete manufacturer' }
    }
  }
}
