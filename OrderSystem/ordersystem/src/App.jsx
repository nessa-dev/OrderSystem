import { useEffect, useState } from 'react'
import axios from 'axios'
import './App.css'

function App() {
    const [pedidos, setPedidos] = useState([])

    useEffect(() => {
        // 1. Pegue a URL do seu Swagger (ex: https://localhost:7001/api/orders)
        axios.get('SUA_URL_DO_SWAGGER_AQUI')
            .then(res => {
                setPedidos(res.data)
            })
            .catch(err => console.error("Erro ao buscar dados:", err))
    }, [])

    return (
        <div style={{ padding: '20px' }}>
            <h1>Lista de Pedidos - OrderSystem</h1>
            <hr />
            {pedidos.length === 0 ? (
                <p>Carregando ou nenhum pedido encontrado...</p>
            ) : (
                <ul>
                    {pedidos.map((p) => (
                        <li key={p.id}>
                            <strong>Pedido #{p.id}</strong> - Status: {p.status}
                        </li>
                    ))}
                </ul>
            )}
        </div>
    )
}

export default App