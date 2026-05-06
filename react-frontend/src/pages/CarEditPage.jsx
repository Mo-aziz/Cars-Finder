import React, { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { carService } from '../services/carService'
import { manufacturerService } from '../services/manufacturerService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'
import Loading from '../components/Loading'

const CarEditPage = () => {
  const { id } = useParams()
  const { isAdmin } = useAuth()
  const [formData, setFormData] = useState({
    brand: '',
    model: '',
    year: new Date().getFullYear(),
    manufacturerId: ''
  })
  const [manufacturers, setManufacturers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const navigate = useNavigate()

  // Check authorization
  useEffect(() => {
    if (!isAdmin) {
      navigate('/cars')
    }
  }, [isAdmin, navigate])

  useEffect(() => {
    fetchData()
  }, [id])

  const fetchData = async () => {
    try {
      const [carData, manufacturersData] = await Promise.all([
        carService.getById(id),
        manufacturerService.getAll()
      ])
      
      setFormData({
        brand: carData.brand,
        model: carData.model,
        year: carData.year,
        manufacturerId: carData.manufacturerId
      })
      setManufacturers(manufacturersData)
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
      [name]: name === 'year' || name === 'manufacturerId' ? parseInt(value) : value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSubmitting(true)

    try {
      await carService.update(id, formData)
      navigate('/cars')
    } catch (err) {
      setError(err.message || 'Failed to update car')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <Link to="/cars" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Cars
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Edit Car</h1>

        <Alert message={error} type="error" onClose={() => setError('')} />

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Brand</label>
            <input
              type="text"
              name="brand"
              value={formData.brand}
              onChange={handleChange}
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Model</label>
            <input
              type="text"
              name="model"
              value={formData.model}
              onChange={handleChange}
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Year</label>
            <input
              type="number"
              name="year"
              value={formData.year}
              onChange={handleChange}
              min="1900"
              max="2100"
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Manufacturer</label>
            <select
              name="manufacturerId"
              value={formData.manufacturerId}
              onChange={handleChange}
              className="input-field"
              required
            >
              <option value="">Select a manufacturer</option>
              {manufacturers.map(m => (
                <option key={m.id} value={m.id}>
                  {m.name}
                </option>
              ))}
            </select>
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="submit"
              disabled={submitting}
              className="btn-primary disabled:opacity-50"
            >
              {submitting ? 'Updating...' : 'Update Car'}
            </button>
            <Link to="/cars" className="btn-secondary inline-block">
              Cancel
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

export default CarEditPage
