import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { carEngineService } from '../services/carEngineService'
import { useAuth } from '../context/AuthContext'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarEnginesPage = () => {
  const [carEngines, setCarEngines] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const { isAdmin, isEmployee } = useAuth()

  useEffect(() => {
    fetchCarEngines()
  }, [])

  const fetchCarEngines = async () => {
    try {
      setLoading(true)
      const data = await carEngineService.getAll()
      setCarEngines(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch car engines')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (carId, engineId) => {
    if (window.confirm('Are you sure you want to delete this car engine relationship?')) {
      try {
        await carEngineService.delete(carId, engineId)
        setSuccess('Car engine deleted successfully!')
        setCarEngines(carEngines.filter(ce => !(ce.carId === carId && ce.engineId === engineId)))
        setTimeout(() => setSuccess(''), 3000)
      } catch (err) {
        setError(err.message || 'Failed to delete car engine')
      }
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-4xl font-bold">Car Engines</h1>
        {(isAdmin || isEmployee) && (
          <Link to="/car-engines/new" className="btn-primary">
            Add Car Engine
          </Link>
        )}
      </div>

      <Alert message={error} type="error" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      {carEngines.length === 0 ? (
        <div className="text-center py-12 animate-fade-in">
          <p className="text-gray-400 text-lg">No car engines found.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {carEngines.map((ce, index) => (
            <div 
              key={`${ce.carId}-${ce.engineId}`} 
              className="card flex justify-between items-center"
              style={{ animationDelay: `${index * 50}ms` }}
            >
              <div>
                <h2 className="text-xl font-bold text-blue-400">
                  {ce.carName} - {ce.engineType}
                </h2>
                <p className="text-gray-400 mt-1">
                  {ce.installationDate ? new Date(ce.installationDate).toLocaleDateString() : 'N/A'}
                </p>
              </div>
              <div className="flex gap-2">
                <Link
                  to={`/car-engines/${ce.carId}/${ce.engineId}`}
                  className="btn-secondary text-sm"
                >
                  View
                </Link>
                {isAdmin && (
                  <>
                    <Link
                      to={`/car-engines/${ce.carId}/${ce.engineId}/edit`}
                      className="btn-secondary text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(ce.carId, ce.engineId)}
                      className="btn-danger text-sm"
                    >
                      Delete
                    </button>
                  </>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default CarEnginesPage
