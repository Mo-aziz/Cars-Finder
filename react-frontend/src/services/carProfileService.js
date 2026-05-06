import api from './api'

export const carProfileService = {
  getAll: async () => {
    try {
      const response = await api.get('/carprofiles')
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch car profiles' }
    }
  },

  getById: async (id) => {
    try {
      const response = await api.get(`/carprofiles/${id}`)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to fetch car profile' }
    }
  },

  create: async (profileData) => {
    try {
      const response = await api.post('/carprofiles', profileData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to create car profile' }
    }
  },

  uploadPhoto: async (file) => {
    try {
      const formData = new FormData()
      formData.append('file', file)
      const response = await api.post('/carprofiles/upload-photo', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      })
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to upload photo' }
    }
  },

  update: async (id, profileData) => {
    try {
      const response = await api.put(`/carprofiles/${id}`, profileData)
      return response.data
    } catch (error) {
      throw error.response?.data || { message: 'Failed to update car profile' }
    }
  },

  delete: async (id) => {
    try {
      await api.delete(`/carprofiles/${id}`)
      return { message: 'Car profile deleted successfully' }
    } catch (error) {
      throw error.response?.data || { message: 'Failed to delete car profile' }
    }
  }
}
