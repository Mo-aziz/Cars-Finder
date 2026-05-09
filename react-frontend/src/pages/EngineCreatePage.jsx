import React, { useState, useEffect } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { engineService } from '../services/engineService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'

const EngineCreatePage = () => {
  const [formData, setFormData] = useState({
    type: '',
    horsepower: ''
  })
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const { isAdmin, isEmployee } = useAuth()
  const navigate = useNavigate()

  // Check authorization
  useEffect(() => {
    if (!isAdmin && !isEmployee) {
      navigate('/engines')
    }
  }, [isAdmin, isEmployee, navigate])

  const handleChange = (e) => {
    const { name, value } = e.target
    setFormData(prev => ({
      ...prev,
      [name]: name === 'horsepower' ? parseInt(value) || '' : value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSubmitting(true)

    try {
      await engineService.create(formData)
      navigate('/engines')
    } catch (err) {
      setError(err.message || 'Failed to create engine')
    } finally {
      setSubmitting(false)
    }
  }

  return (
    <div className="container">
      <Link to="/engines" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Engines
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Add New Engine</h1>

        <Alert message={error} type="error" onClose={() => setError('')} />

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium mb-2">Type</label>
            <input
              type="text"
              name="type"
              value={formData.type}
              onChange={handleChange}
              placeholder="e.g., V6, V8, Inline-4"
              className="input-field"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Horsepower</label>
            <input
              type="number"
              name="horsepower"
              value={formData.horsepower}
              onChange={handleChange}
              placeholder="e.g., 300"
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
              {submitting ? 'Creating...' : 'Create Engine'}
            </button>
            <Link to="/engines" className="btn-secondary inline-block">
              Cancel
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

export default EngineCreatePage
