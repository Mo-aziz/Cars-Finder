import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import { manufacturerService } from '../services/manufacturerService'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const ManufacturerDetailsPage = () => {
  const { id } = useParams()
  const [manufacturer, setManufacturer] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    fetchManufacturer()
  }, [id])

  const fetchManufacturer = async () => {
    try {
      setLoading(true)
      const data = await manufacturerService.getById(id)
      setManufacturer(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch manufacturer')
    } finally {
      setLoading(false)
    }
  }

  if (loading) return <Loading />

  if (!manufacturer) {
    return (
      <div className="container">
        <Alert message="Manufacturer not found" type="error" />
        <Link to="/manufacturers" className="btn-secondary inline-block">
          Back to Manufacturers
        </Link>
      </div>
    )
  }

  return (
    <div className="container">
      <Link to="/manufacturers" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Manufacturers
      </Link>

      <Alert message={error} type="error" onClose={() => setError('')} />

      <div className="card max-w-2xl">
        <h1 className="text-4xl font-bold mb-6 text-blue-400">{manufacturer.name}</h1>
        
        <div className="space-y-4">
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Country</p>
            <p className="text-2xl font-semibold">{manufacturer.country}</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default ManufacturerDetailsPage
