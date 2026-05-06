import React, { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { carService } from '../services/carService'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarDetailsPage = () => {
  const { id } = useParams()
  const [car, setCar] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    fetchCar()
  }, [id])

  const fetchCar = async () => {
    try {
      setLoading(true)
      const data = await carService.getById(id)
      setCar(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch car')
    } finally {
      setLoading(false)
    }
  }

  if (loading) return <Loading />

  if (!car) {
    return (
      <div className="container">
        <Alert message="Car not found" type="error" />
        <Link to="/cars" className="btn-secondary inline-block">
          Back to Cars
        </Link>
      </div>
    )
  }

  return (
    <div className="container">
      <Link to="/cars" className="text-blue-400 hover:text-blue-300 mb-4 inline-block">
        ← Back to Cars
      </Link>

      <Alert message={error} type="error" onClose={() => setError('')} />

      <div className="card max-w-2xl">
        <h1 className="text-4xl font-bold mb-6 text-blue-400">{car.brand} {car.model}</h1>
        
        <div className="space-y-4">
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Year</p>
            <p className="text-2xl font-semibold">{car.year}</p>
          </div>
          
          <div className="border-b border-gray-700 pb-3 transition duration-300">
            <p className="text-gray-400 text-sm uppercase tracking-wide">Manufacturer</p>
            <p className="text-2xl font-semibold">{car.manufacturerName}</p>
          </div>

          {car.carProfile && (
            <>
              <div className="border-b border-gray-700 pb-3 transition duration-300">
                <p className="text-gray-400 text-sm uppercase tracking-wide">Color</p>
                <p className="text-2xl font-semibold">{car.carProfile.color}</p>
              </div>
              
              <div className="border-b border-gray-700 pb-3 transition duration-300">
                <p className="text-gray-400 text-sm uppercase tracking-wide">Price</p>
                <p className="text-2xl font-semibold">${car.carProfile.price?.toLocaleString()}</p>
              </div>
              
              <div className="transition duration-300">
                <p className="text-gray-400 text-sm uppercase tracking-wide">Description</p>
                <p className="text-lg mt-2">{car.carProfile.description}</p>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  )
}

export default CarDetailsPage
