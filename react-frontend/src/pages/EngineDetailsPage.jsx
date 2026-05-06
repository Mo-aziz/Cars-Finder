import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { engineService } from '../services/engineService'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const EngineDetailsPage = () => {
  const { id } = useParams()
  const [engine, setEngine] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    fetchEngine()
  }, [id])

  const fetchEngine = async () => {
    try {
      setLoading(true)
      const data = await engineService.getById(id)
      setEngine(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch engine')
    } finally {
      setLoading(false)
    }
  }

  if (loading) return <Loading />

  if (!engine) {
    return (
      <div className="container">
        <Alert message="Engine not found" type="error" />
        <Link to="/engines" className="btn-secondary inline-block">
          Back to Engines
        </Link>
      </div>
    )
  }

  return (
    <div className="container">
      <Link to="/engines" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Engines
      </Link>

      <Alert message={error} type="error" onClose={() => setError('')} />

      <div className="card max-w-2xl">
        <h1 className="text-4xl font-bold mb-6 text-blue-400">{engine.type}</h1>
        
        <div className="space-y-4">
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Horsepower</p>
            <p className="text-2xl font-semibold">{engine.horsePower} HP</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default EngineDetailsPage
