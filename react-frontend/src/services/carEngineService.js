import api from './api'

export const carEngineService = {
  getAll: async () => {
    try {
      const response = await api.get('/carengines')
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch car engines' }
    }
  },

  getByIds: async (carId, engineId) => {
    try {
      const response = await api.get(`/carengines/${carId}/${engineId}`)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch car engine' }
    }
  },

  create: async (carEngineData) => {
    try {
      const response = await api.post('/carengines', carEngineData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to create car engine' }
    }
  },

  update: async (carId, engineId, carEngineData) => {
    try {
      const response = await api.put(`/carengines/${carId}/${engineId}`, carEngineData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to update car engine' }
    }
  },

  delete: async (carId, engineId) => {
    try {
      await api.delete(`/carengines/${carId}/${engineId}`)
      return { message: 'Car engine deleted successfully' }
    } catch (error) {
      throw error.response?.data || { message: 'Failed to delete car engine' }
    }
  }
}
