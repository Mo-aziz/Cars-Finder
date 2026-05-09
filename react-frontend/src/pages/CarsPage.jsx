import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { carService } from '../services/carService'
import { useAuth } from '../context/AuthContext'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarsPage = () => {
  const [cars, setCars] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const { isAdmin, isEmployee } = useAuth()

  useEffect(() => {
    fetchCars()
  }, [])

  const fetchCars = async () => {
    try {
      setLoading(true)
      const data = await carService.getAll()
      setCars(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch cars')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this car?')) {
      try {
        await carService.delete(id)
        setSuccess('Car deleted successfully!')
        setCars(cars.filter(car => car.id !== id))
        setTimeout(() => setSuccess(''), 3000)
      } catch (err) {
        setError(err.message || 'Failed to delete car')
      }
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-4xl font-bold">Cars</h1>
        {(isAdmin || isEmployee) && (
          <Link to="/cars/new" className="btn-primary">
            Add Car
          </Link>
        )}
      </div>

      <Alert message={error} type="error" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      {cars.length === 0 ? (
        <div className="text-center py-12 animate-fade-in">
          <p className="text-gray-400 text-lg\">No cars found.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {cars.map((car, index) => (
            <div 
              key={car.id} 
              className="card flex justify-between items-center"
              style={{ animationDelay: `${index * 50}ms` }}
            >
              <div>
                <h2 className="text-xl font-bold text-blue-400">
                  {car.brand} {car.model}
                </h2>
                <p className="text-gray-400 mt-1">
                  {car.year} | {car.manufacturerName}
                </p>
              </div>
              <div className="flex gap-2">
                <Link
                  to={`/cars/${car.id}`}
                  className="btn-secondary text-sm"
                >
                  View
                </Link>
                {isAdmin && (
                  <>
                    <Link
                      to={`/cars/${car.id}/edit`}
                      className="btn-secondary text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(car.id)}
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

export default CarsPage
