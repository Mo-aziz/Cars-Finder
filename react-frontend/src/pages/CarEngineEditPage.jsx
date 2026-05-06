import React, { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { carEngineService } from '../services/carEngineService'
import { carService } from '../services/carService'
import { engineService } from '../services/engineService'
import { useAuth } from '../context/AuthContext'
import Alert from '../components/Alert'
import Loading from '../components/Loading'

const CarEngineEditPage = () => {
  const { carId, engineId } = useParams()
  const { isAdmin } = useAuth()
  const [formData, setFormData] = useState({
    carId: '',
    engineId: '',
    installationDate: '',
    notes: ''
  })
  const [cars, setCars] = useState([])
  const [engines, setEngines] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [submitting, setSubmitting] = useState(false)
  const navigate = useNavigate()

  // Check authorization
  useEffect(() => {
    if (!isAdmin) {
      navigate('/car-engines')
    }
  }, [isAdmin, navigate])

  useEffect(() => {
    fetchData()
  }, [carId, engineId])

  const fetchData = async () => {
    try {
      const [ceData, carsData, enginesData] = await Promise.all([
        carEngineService.getByIds(carId, engineId),
        carService.getAll(),
        engineService.getAll()
      ])
      
      setFormData({
        carId: ceData.carId,
        engineId: ceData.engineId,
        installationDate: ceData.installationDate ? ceData.installationDate.split('T')[0] : '',
        notes: ceData.notes || ''
      })
      setCars(carsData)
      setEngines(enginesData)
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
      [name]: name === 'carId' || name === 'engineId' ? parseInt(value) || '' : value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setError('')
    setSubmitting(true)

    try {
      await carEngineService.update(carId, engineId, formData)
      navigate('/car-engines')
    } catch (err) {
      setError(err.message || 'Failed to update car engine')
    } finally {
      setSubmitting(false)
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <Link to="/car-engines" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Car Engines
      </Link>

      <div className="card max-w-2xl mx-auto">
        <h1 className="text-3xl font-bold mb-6">Edit Car Engine</h1>

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
            <label className="block text-sm font-medium mb-2">Engine</label>
            <select
              name="engineId"
              value={formData.engineId}
              onChange={handleChange}
              className="input-field"
              required
            >
              <option value="">Select an engine</option>
              {engines.map(engine => (
                <option key={engine.id} value={engine.id}>
                  {engine.type} ({engine.horsepower} HP)
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Installation Date</label>
            <input
              type="date"
              name="installationDate"
              value={formData.installationDate}
              onChange={handleChange}
              className="input-field"
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-2">Notes</label>
            <textarea
              name="notes"
              value={formData.notes}
              onChange={handleChange}
              className="input-field"
              rows="4"
            />
          </div>

          <div className="flex gap-4 pt-4">
            <button
              type="submit"
              disabled={submitting}
              className="btn-primary disabled:opacity-50"
            >
              {submitting ? 'Updating...' : 'Update Car Engine'}
            </button>
            <Link to="/car-engines" className="btn-secondary inline-block">
              Cancel
            </Link>
          </div>
        </form>
      </div>
    </div>
  )
}

export default CarEngineEditPage
