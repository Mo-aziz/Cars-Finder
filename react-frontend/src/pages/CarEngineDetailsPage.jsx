import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { carEngineService } from '../services/carEngineService'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarEngineDetailsPage = () => {
  const { carId, engineId } = useParams()
  const [carEngine, setCarEngine] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    fetchCarEngine()
  }, [carId, engineId])

  const fetchCarEngine = async () => {
    try {
      setLoading(true)
      const data = await carEngineService.getByIds(carId, engineId)
      setCarEngine(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch car engine')
    } finally {
      setLoading(false)
    }
  }

  if (loading) return <Loading />

  if (!carEngine) {
    return (
      <div className="container">
        <Alert message="Car engine not found" type="error" />
        <Link to="/car-engines" className="btn-secondary inline-block">
          Back to Car Engines
        </Link>
      </div>
    )
  }

  return (
    <div className="container">
      <Link to="/car-engines" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Car Engines
      </Link>

      <Alert message={error} type="error" onClose={() => setError('')} />

      <div className="card max-w-2xl">
        <h1 className="text-4xl font-bold mb-6 text-blue-400">
          {carEngine.carName} - {carEngine.engineType}
        </h1>
        
        <div className="space-y-4">
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Car</p>
            <p className="text-xl font-semibold">{carEngine.carName}</p>
          </div>

          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Engine</p>
            <p className="text-xl font-semibold">{carEngine.engineType}</p>
          </div>

          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Horsepower</p>
            <p className="text-xl font-semibold">{carEngine.engineHorsePower} HP</p>
          </div>

          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Installation Date</p>
            <p className="text-xl font-semibold">
              {carEngine.installationDate ? new Date(carEngine.installationDate).toLocaleDateString() : 'N/A'}
            </p>
          </div>

          {carEngine.notes && (
            <div className="transition duration-300">
              <p className="text-gray-400 text-sm uppercase tracking-wide">Notes</p>
              <p className="text-lg mt-2">{carEngine.notes}</p>
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

export default CarEngineDetailsPage
