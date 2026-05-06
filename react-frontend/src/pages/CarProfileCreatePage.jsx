import React, { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { carProfileService } from '../services/carProfileService'
import { carService } from '../services/carService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'
import Loading from '../components/Loading'

const CarProfileCreatePage = () => {
  const [formData, setFormData] = useState({
    carId: '',
    color: '',
    price: '',
    description: '',
    photoUrl: ''
  })
  const [cars, setCars] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const [uploadingPhoto, setUploadingPhoto] = useState(false)
  const { isAdmin, isEmployee } = useAuth()
  const navigate = useNavigate()
  const defaultCarIcon = '/default-car.svg'

  // Check authorization
  useEffect(() => {
    if (!isAdmin && !isEmployee) {
      navigate('/car-profiles')
    }
  }, [isAdmin, isEmployee, navigate])

  useEffect(() => {
    fetchCars()
  }, [])

  const fetchCars = async () => {
    try {
      const data = await carService.getAll()
      setCars(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch cars')
    } finally {
      setLoading(false)
    }
  }

  const handleChange = (e) => {
    const { name, value } = e.target
    let newValue = value
    
    if (name === 'carId') {
      newValue = value ? parseInt(value) : ''
    } else if (name === 'price') {
      newValue = value ? parseFloat(value) : null
    }
    
    setFormData(prev => ({
      ...prev,
      [name]: newValue
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    
    // Validate required fields
    if (!formData.carId) {
      setError('Car is required')
      return
    }
    
    setSubmitting(true)

    try {
      // Clean up the data before sending
      const dataToSend = {
        carId: formData.carId,
        color: formData.color || null,
        price: formData.price !== null && formData.price !== '' ? formData.price : null,
        description: formData.description || null,
        photoUrl: formData.photoUrl || null
      }
      
      await carProfileService.create(dataToSend)
      navigate('/car-profiles')
    } catch (err) {
      setError(err.message || 'Failed to create car profile')
    } finally {
      setSubmitting(false)
    }
  }

  const handlePhotoSelect = async (e) => {
    const file = e.target.files?.[0]
    if (!file) return

    setError('')
    setUploadingPhoto(true)
    try {
      const result = await carProfileService.uploadPhoto(file)
      setFormData((prev) => ({ ...prev, photoUrl: result.photoUrl || '' }))
    } catch (err) {
      setError(err.message || 'Failed to upload photo')
    } finally {
      setUploadingPhoto(false)
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <Link to="/car-profiles" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Car Profiles
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Add New Car Profile</h1>

        <Alert message={error} type="error" onClose={() => setError('')} />

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Car</label>
            <select
              name="carId"
              value={formData.carId}
              onChange={handleChange}
              className="input-field"
              required
            >
              <option value="">Select a car</option>
              {cars.map(car => (
                <option key={car.id} value={car.id}>
                  {car.brand} {car.model} ({car.year})
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Color</label>
            <input
              type="text"
              name="color"
              value={formData.color}
              onChange={handleChange}
              placeholder="e.g., Red, Blue, Black"
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Price</label>
            <input
              type="number"
              name="price"
              value={formData.price}
              onChange={handleChange}
              placeholder="e.g., 25000"
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Description</label>
            <textarea
              name="description"
              value={formData.description}
              onChange={handleChange}
              placeholder="Add a description..."
              className="input-field"
              rows="4"
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Photo URL</label>
            <input
              type="url"
              name="photoUrl"
              value={formData.photoUrl}
              onChange={handleChange}
              placeholder="https://example.com/car.jpg"
              className="input-field"
            />
            <p className="text-xs text-gray-400 mt-1">Or upload from your device:</p>
            <input
              type="file"
              accept="image/*"
              onChange={handlePhotoSelect}
              className="input-field mt-2"
            />
            {uploadingPhoto && <p className="text-sm text-gray-400 mt-2">Uploading photo...</p>}
            <img
              src={formData.photoUrl || defaultCarIcon}
              alt="Car preview"
              className="w-28 h-28 rounded object-cover border border-gray-700 mt-3"
              onError={(ev) => {
                ev.currentTarget.onerror = null
                ev.currentTarget.src = defaultCarIcon
              }}
            />
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="submit"
              disabled={submitting}
              className="btn-primary disabled:opacity-50"
            >
              {submitting ? 'Creating...' : 'Create Car Profile'}
            </button>
            <Link to="/car-profiles" className="btn-secondary inline-block">
              Cancel
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

export default CarProfileCreatePage
