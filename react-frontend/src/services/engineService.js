import api from './api'

export const engineService = {
  getAll: async () => {
    try {
      const response = await api.get('/engine')
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch engines' }
    }
  },

  getById: async (id) => {
    try {
      const response = await api.get(`/engine/${id}`)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch engine' }
    }
  },

  create: async (engineData) => {
    try {
      const response = await api.post('/engine', engineData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to create engine' }
    }
  },

  update: async (id, engineData) => {
    try {
      const response = await api.put(`/engine/${id}`, engineData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to update engine' }
    }
  },

  delete: async (id) => {
    try {
      await api.delete(`/engine/${id}`)
      return { message: 'Engine deleted successfully' }
    } catch (error) {
      throw error.response?.data || { message: 'Failed to delete engine' }
    }
  }
}
