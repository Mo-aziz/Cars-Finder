import React, { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { carProfileService } from '../services/carProfileService'
import { carService } from '../services/carService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'
import Loading from '../components/Loading'

const CarProfileEditPage = () => {
  const { id } = useParams()
  const { isAdmin, isEmployee } = useAuth()
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
  const navigate = useNavigate()

  // Check authorization
  useEffect(() => {
    if (!isAdmin && !isEmployee) {
      navigate('/car-profiles')
    }
  }, [isAdmin, isEmployee, navigate])

  useEffect(() => {
    fetchData()
  }, [id])

  const fetchData = async () => {
    try {
      const [profileData, carsData] = await Promise.all([
        carProfileService.getById(id),
        carService.getAll()
      ])
      
      setFormData({
        carId: profileData.carId,
        color: profileData.color,
        price: profileData.price,
        description: profileData.description || '',
        photoUrl: profileData.photoUrl || ''
      })
      setCars(carsData)
    } catch (err) {
      setError(err.message || 'Failed to fetch data')
    } finally {
      setLoading(false)
    }
  }

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'carId' ? parseInt(value) || '' : name === 'price' ? parseFloat(value) || '' : value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSubmitting(true)

    try {
      await carProfileService.update(id, formData)
      navigate('/car-profiles')
    } catch (err) {
      setError(err.message || 'Failed to update car profile')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <Link to="/car-profiles" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Car Profiles
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Edit Car Profile</h1>

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
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="submit"
              disabled={submitting}
              className="btn-primary disabled:opacity-50"
            >
              {submitting ? 'Updating...' : 'Update Car Profile'}
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

export default CarProfileEditPage
