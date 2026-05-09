import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { manufacturerService } from '../services/manufacturerService'
import { useAuth } from '../context/AuthContext'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const ManufacturersPage = () => {
  const [manufacturers, setManufacturers] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const { isAdmin, isEmployee } = useAuth()

  useEffect(() => {
    fetchManufacturers()
  }, [])

  const fetchManufacturers = async () => {
    try {
      setLoading(true)
      const data = await manufacturerService.getAll()
      setManufacturers(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch manufacturers')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this manufacturer?')) {
      try {
        await manufacturerService.delete(id)
        setSuccess('Manufacturer deleted successfully!')
        setManufacturers(manufacturers.filter(m => m.id !== id))
        setTimeout(() => setSuccess(''), 3000)
      } catch (err) {
        setError(err.message || 'Failed to delete manufacturer')
      }
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-4xl font-bold">Manufacturers</h1>
        {(isAdmin || isEmployee) && (
          <Link to="/manufacturers/new" className="btn-primary">
            Add Manufacturer
          </Link>
        )}
      </div>

      <Alert message={error} type="error" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      {manufacturers.length === 0 ? (
        <div className="text-center py-12 animate-fade-in">
          <p className="text-gray-400 text-lg">No manufacturers found.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {manufacturers.map((manufacturer, index) => (
            <div 
              key={manufacturer.id} 
              className="card flex justify-between items-center"
              style={{ animationDelay: `${index * 50}ms` }}
            >
              <div>
                <h2 className="text-xl font-bold text-blue-400">{manufacturer.name}</h2>
                <p className="text-gray-400 mt-1">{manufacturer.country}</p>
              </div>
              <div className="flex gap-2">
                <Link
                  to={`/manufacturers/${manufacturer.id}`}
                  className="btn-secondary text-sm"
                >
                  View
                </Link>
                {isAdmin && (
                  <>
                    <Link
                      to={`/manufacturers/${manufacturer.id}/edit`}
                      className="btn-secondary text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(manufacturer.id)}
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

export default ManufacturersPage
