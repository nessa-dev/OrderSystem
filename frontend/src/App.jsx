import { useEffect, useState } from 'react'
import axios from 'axios'

export default function App() {
    const [orders, setOrders] = useState([])
    const [customerName, setCustomerName] = useState('')

    // Configuração base do Axios para não repetir a URL
    const api = axios.create({ baseURL: 'https://localhost:7114/api' })

    // Função para carregar os pedidos (GET)
    const loadOrders = async () => {
        try {
            const res = await api.get('/orders')
            console.log("Dados recebidos:", res.data)
            setOrders(res.data)
        } catch (err) {
            console.error("Erro ao carregar pedidos:", err)
        }
    }

    // Carrega ao abrir a tela
    useEffect(() => {
        loadOrders();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    // Função para Criar Pedido (POST)
    // Função para Criar Pedido (POST)
    const handleCreateOrder = async (e) => {
        e.preventDefault();
        if (!customerName) return alert("Digite o nome do cliente!");

        try {
            // AJUSTE: Enviando 'CustomerName' e inicializando o TotalAmount se necessário
            await api.post('/orders', {
                CustomerName: customerName,
                TotalAmount: 0 // Adicionei isso porque vi no seu erro do C#
            });

            setCustomerName('');
            loadOrders();
            alert("Pedido criado com sucesso!");
        } catch (err) {
            // Se der erro, vamos ver EXATAMENTE o que o C# disse
            console.error("Erro do C#:", err.response?.data);
            alert("Erro ao criar: " + (err.response?.data?.title || "Verifique o console"));
        }
    };

    // Função para Deletar Pedido (DELETE)
    const handleDeleteOrder = async (id) => {
        if (!confirm(`Tem certeza que deseja excluir o pedido #${id}?`)) return

        try {
            // Chama DELETE /api/orders/{id}
            await api.delete(`/orders/${id}`)
            loadOrders() // Atualiza a lista
            alert("Pedido excluído!")
        } catch (err) {
            console.error("Erro ao deletar:", err.response?.data || err.message)
            alert("Erro ao excluir pedido.")
        }
    }

    // Função para Finalizar Pedido (POST)
    const handleFinalizeOrder = async (id) => {
        try {
            // No C#, Finalize geralmente não recebe corpo (null)
            // O segredo é o responseType: 'text' para o Ok("Finalizado")
            await api.post(`/orders/${id}/finalize`, null, {
                responseType: 'text'
            });

            loadOrders();
            alert("Pedido finalizado!");
        } catch (err) {
            console.error("Erro ao finalizar:", err.response?.data);
            alert("Erro ao finalizar. O pedido já pode estar finalizado.");
        }
    };

    // Estilos rápidos (mantendo o visual da sua imagem)
    const styles = {
        container: { padding: '40px', backgroundColor: '#121212', minHeight: '100vh', color: 'white', fontFamily: 'sans-serif' },
        form: { marginBottom: '30px', display: 'flex', gap: '10px' },
        input: { padding: '12px', flex: 1, borderRadius: '4px', border: '1px solid #333', backgroundColor: '#222', color: 'white' },
        btnSuccess: { padding: '12px 20px', backgroundColor: '#4caf50', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' },
        grid: { display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '20px' },
        card: { background: '#1e1e1e', padding: '20px', borderRadius: '8px', borderLeft: '5px solid #646cff', boxShadow: '0 4px 6px rgba(0,0,0,0.3)' },
        btnAction: { padding: '8px 15px', border: 'none', borderRadius: '4px', cursor: 'pointer', color: 'white', fontWeight: 'bold' }
    }

    return (
        <div style={styles.container}>
            <header style={{ textAlign: 'center', marginBottom: '40px' }}>
                <h1 style={{ fontSize: '3rem' }}>📦 Order System Dashboard</h1>
                <p style={{ color: '#888' }}>Conectado a: https://localhost:7114</p>
            </header>

            {/* Formulário de Criação */}
            <form onSubmit={handleCreateOrder} style={styles.form}>
                <input
                    type="text"
                    placeholder="Nome do Cliente"
                    value={customerName}
                    onChange={(e) => setCustomerName(e.target.value)}
                    style={styles.input}
                />
                <button type="submit" style={styles.btnSuccess}>
                    Criar Pedido
                </button>
            </form>

            {/* Listagem de Pedidos */}
            <div style={styles.grid}>
                {orders.map(order => (
                    <div key={order.id} style={styles.card}>
                        <h3 style={{ margin: '0 0 15px 0', borderBottom: '1px solid #333', paddingBottom: '10px' }}>
                            Pedido #{order.id}
                        </h3>
                        <p>Cliente: <strong>{order.customerName || 'N/A'}</strong></p>
                        <p style={{ color: '#4caf50', fontSize: '1.2rem' }}>Total: ${order.totalValue?.toFixed(2) || '0.00'}</p>

                        <div style={{ marginTop: '20px', display: 'flex', gap: '10px' }}>
                            <button
                                onClick={() => handleFinalizeOrder(order.id)}
                                style={{ ...styles.btnAction, backgroundColor: '#2196f3' }}>
                                Finalizar
                            </button>
                            <button
                                onClick={() => handleDeleteOrder(order.id)}
                                style={{ ...styles.btnAction, backgroundColor: '#f44336' }}>
                                Excluir
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    )
}