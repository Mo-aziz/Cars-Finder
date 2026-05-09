import React, { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { manufacturerService } from '../services/manufacturerService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'

const ManufacturerCreatePage = () => {
  const [formData, setFormData] = useState({
    name: '',
    country: ''
  })
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const { isAdmin, isEmployee } = useAuth()
  const navigate = useNavigate()

  // Check authorization
  React.useEffect(() => {
    if (!isAdmin && !isEmployee) {
      navigate('/manufacturers')
    }
  }, [isAdmin, isEmployee, navigate])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSubmitting(true)

    try {
      await manufacturerService.create(formData)
      navigate('/manufacturers')
    } catch (err) {
      setError(err.message || 'Failed to create manufacturer')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div className="container">
      <Link to="/manufacturers" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Manufacturers
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Add New Manufacturer</h1>

        <Alert message={error} type="error" onClose={() => setError('')} />

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Name</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Country</label>
            <input
              type="text"
              name="country"
              value={formData.country}
              onChange={handleChange}
              className="input-field"
              required
            />
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="submit"
              disabled={submitting}
              className="btn-primary disabled:opacity-50"
            >
              {submitting ? 'Creating...' : 'Create Manufacturer'}
            </button>
            <Link to="/manufacturers" className="btn-secondary inline-block">
              Cancel
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

export default ManufacturerCreatePage
