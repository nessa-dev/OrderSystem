import { useEffect, useState } from 'react';
import axios from 'axios';
import { GrEdit } from "react-icons/gr";
import { FaTrash } from "react-icons/fa";
export default function App() {
    // --- States ---
    const [orders, setOrders] = useState([]);
    const [products, setProducts] = useState([]);
    const [customerName, setCustomerName] = useState('');
    const [newProductName, setNewProductName] = useState('');
    const [newProductPrice, setNewProductPrice] = useState('');
    const [selectedProductId, setSelectedProductId] = useState('');
    const [quantity, setQuantity] = useState(1);
    const [tempItems, setTempItems] = useState([]);
    const [activeTab, setActiveTab] = useState('open'); 
    const api = axios.create({ baseURL: 'https://localhost:7114/api' });

    // --- Filters (Status: 1=Open, 2=Finalized, 3=Cancelled) ---
    const openOrders = orders.filter(o => o.status === 1);
    const finalizedOrders = orders.filter(o => o.status === 2);
    const cancelledOrders = orders.filter(o => o.status === 3);

    const loadData = async () => {
        try {
            const [ordersRes, productsRes] = await Promise.all([
                api.get('/orders'),
                api.get('/products')
            ]);
            setOrders(ordersRes.data);
            setProducts(productsRes.data);
        } catch (err) { console.error("API Error:", err); }
    };

    useEffect(() => {
        document.body.style.backgroundColor = '#f07592';
        document.body.style.margin = '0';
        document.body.style.padding = '0';
        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const handleCreateProduct = async (e) => {
        e.preventDefault();
        const price = parseFloat(newProductPrice);
        if (!newProductName || isNaN(price) || price <= 0) return alert("Invalid dessert data.");
        try {
            await api.post('/products', { Name: newProductName, Price: price });
            setNewProductName(''); setNewProductPrice('');
            await loadData();
            alert("Dessert added to the menu!");
        } catch { alert("Error saving the dessert."); }
    };

    const handleDeleteProduct = async (id) => {
        if (!window.confirm("Are you sure you want to remove this dessert from the menu?")) return;
        try { await api.delete(`/products/${id}`); await loadData(); alert("Dessert removed!"); } catch { alert("Error removing: check if it is used in any order."); }
    };

    const handleEditProduct = async (id, currentName, currentPrice) => {
        const newName = prompt("New dessert name:", currentName);
        const newPrice = prompt("New price:", currentPrice);
        if (newName && newPrice) {
            try {
                await api.put(`/products/${id}`, { Name: newName, Price: parseFloat(newPrice) });
                await loadData();
                alert("Dessert updated!");
            } catch { alert("Error updating the dessert."); }
        }
    };

    // --- Sales Logic (Orders) ---
    const addToTempOrder = () => {
        if (!selectedProductId) return alert("Select a dessert!");
        const product = products.find(p => p.id === parseInt(selectedProductId));
        const qty = parseInt(quantity);
        if (isNaN(qty) || qty <= 0) return alert("Invalid quantity.");
        setTempItems([...tempItems, { ...product, quantity: qty }]);
        setSelectedProductId(''); setQuantity(1); 
    };

    const handleCreateOrder = async (e) => {
        e.preventDefault();
        if (!customerName || tempItems.length === 0) return alert("Customer name and at least 1 dessert are required!");
        try {
            const res = await api.post('/orders', { CustomerName: customerName });
            const orderId = res.data.id;
            for (const item of tempItems) {
                await api.post(`/orders/${orderId}/products`, { ProductId: item.id, Quantity: item.quantity });
            }
            setCustomerName(''); setTempItems([]);
            setTimeout(async () => { await loadData(); setActiveTab('open'); alert("Order successfully created!"); }, 500);
        } catch { alert("Error creating order."); }
    };

    const updateStatus = async (id, action, msg) => {
        if (!window.confirm(`Do you want to ${msg} this order?`)) return;
        try {
            await api.post(`/orders/${id}/${action}`);
            await loadData();
            alert(`Order ${msg}d successfully!`);
        } catch { alert(`Failed to ${msg} order.`); }
    };

    const colors = {
        bg: '#e9e5d4',       
        brown: '#7e4414',   
        pink: '#f07592',     
        lightPink: '#fba4bf',
        text: '#5d310e',    
        white: '#ffffff',    
        green: '#a7d7a9',   
        cancelled: '#d32f2f' 
    };

    const styles = {
        container: { padding: '40px', backgroundColor: colors.bg, minHeight: '100vh', color: colors.brown, fontFamily: '"Segoe UI", Roboto, Helvetica, Arial, sans-serif', boxSizing: 'border-box' },
        header: { textAlign: 'center', marginBottom: '40px', borderBottom: `2px dashed ${colors.pink}`, paddingBottom: '20px' },
        nav: { display: 'flex', gap: '10px', marginBottom: '30px', flexWrap: 'wrap', justifyContent: 'center' },

        // Tab Button Style
        tabBtn: (active, activeColor) => ({
            padding: '12px 24px', backgroundColor: active ? activeColor : colors.white, color: active ? colors.white : colors.brown,
            border: `2px solid ${activeColor}`, borderRadius: '25px', cursor: 'pointer', fontWeight: 'bold', fontSize: '16px',
            transition: 'all 0.3s ease', boxShadow: active ? '0 4px 6px rgba(0,0,0,0.1)' : 'none',
        }),

        // Card Style
        card: {
            background: colors.white, padding: '25px', borderRadius: '15px', marginBottom: '20px',
            boxShadow: '0 6px 15px rgba(126, 68, 20, 0.1)', borderLeft: `6px solid ${colors.pink}`, position: 'relative'
        },

        // Input and Select Style
        input: {
            padding: '12px 15px', marginBottom: '15px', width: '100%', borderRadius: '8px',
            border: `1px solid ${colors.lightPink}`, backgroundColor: colors.white, color: colors.brown, fontSize: '16px', boxSizing: 'border-box'
        },

        // Pink Action Button
        btnMain: {
            padding: '12px 20px', backgroundColor: colors.pink, color: colors.white, border: 'none',
            borderRadius: '8px', cursor: 'pointer', fontWeight: 'bold', fontSize: '16px', width: '100%', transition: 'background-color 0.3s'
        },

        // Secondary Action Buttons 
        btnAction: (bgColor) => ({
            padding: '10px 15px', backgroundColor: bgColor, color: colors.white, border: 'none',
            borderRadius: '6px', cursor: 'pointer', fontWeight: 'bold', flex: 1, fontSize: '14px'
        }),

        // Product List inside order
        productList: { fontSize: '15px', color: colors.text, margin: '15px 0', paddingLeft: '25px', lineHeight: '1.6' },
        price: { color: colors.green, fontWeight: 'bold', fontSize: '1.1em' }
    };

    const OrderCard = ({ order, showActions, statusColor, statusText }) => (
        <div key={order.id} style={{ ...styles.card, borderLeftColor: statusColor }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <h3 style={{ margin: 0, color: colors.brown }}>Order #{order.id}</h3>
                <span style={{ fontSize: '14px', fontWeight: 'bold', color: statusColor, backgroundColor: `${statusColor}22`, padding: '5px 10px', borderRadius: '15px' }}>
                    {statusText}
                </span>
            </div>

            <p style={{ margin: '10px 0 5px 0' }}>
                Customer: <strong>{order.customerName}</strong>
            </p>

            <ul style={styles.productList}>
                {order.products && order.products.length > 0 ? (
                    order.products.map((p, i) => (
                        <li key={i}>
                            {p.name}
                            <span style={{ color: '#888' }}> (x{p.quantity || 1})</span>
                        </li>
                    ))
                ) : (
                    <li style={{ listStyle: 'none', marginLeft: '-25px', color: '#999' }}>
                        No desserts listed in this order.
                    </li>
                )}
            </ul>

            {showActions && (
                <div style={{ display: 'flex', gap: '10px', marginTop: '20px' }}>
                    <button onClick={() => updateStatus(order.id, 'finalize', 'finalize')} style={styles.btnAction(colors.pink)}>
                        Complete Order
                    </button>
                    <button onClick={() => updateStatus(order.id, 'cancel', 'cancel')} style={styles.btnAction(colors.cancelled)}>
                        Cancel
                    </button>
                </div>
            )}
        </div>
    );

    return (

        <div style={styles.container}>

            {/* --- LOGO --- */}
            <header style={styles.header}>
                <img
                    src="/logo.png"  
                    alt="Logo Confeitaria"
                    style={{
                        maxHeight: '150px', 
                        display: 'block',
                        margin: '0 auto 10px auto' 
                    }}
                />
            </header>

            <div style={styles.nav}>
                <button style={styles.tabBtn(activeTab === 'catalog', colors.brown)} onClick={() => setActiveTab('catalog')}>Menu (Desserts)</button>
                <button style={styles.tabBtn(activeTab === 'open', colors.pink)} onClick={() => setActiveTab('open')}>Open Orders</button>
                <button style={styles.tabBtn(activeTab === 'finalized', '#2196f3')} onClick={() => setActiveTab('finalized')}>Completed (History)</button>
                <button style={styles.tabBtn(activeTab === 'cancelled', colors.cancelled)} onClick={() => setActiveTab('cancelled')}>Cancelled</button>
            </div>

            {/* --- TAB: MENU --- */}
            {activeTab === 'catalog' && (
                <div>
                    <div style={{ ...styles.card, borderLeftColor: colors.brown }}>
                        <h3>🎂 Add New Dessert to Menu</h3>
                        <form onSubmit={handleCreateProduct}>
                            <input style={styles.input} placeholder="Dessert Name (e.g., Carrot Cake)" value={newProductName} onChange={e => setNewProductName(e.target.value)} />
                            <input style={styles.input} type="number" step="0.01" placeholder="Price ($)" value={newProductPrice} onChange={e => setNewProductPrice(e.target.value)} />
                            <button type="submit" style={{ ...styles.btnMain, backgroundColor: colors.brown }}>Save to Menu</button>
                        </form>
                    </div>

                    <h3>Available Desserts ({products.length})</h3>

                    <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: '20px' }}>
                        {products.map(p => (
                            <div key={p.id} style={{ ...styles.card, borderLeftColor: colors.lightPink, margin: 0, backgroundColor: colors.white }}>
                                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                    <div>
                                        <strong style={{ fontSize: '1.1em', color: colors.brown }}>{p.name}</strong>
                                        <p style={styles.price}>$ {p.price?.toFixed(2)}</p>
                                    </div>

                                    <div style={{ display: 'flex', gap: '8px' }}>
                                        <button
                                            onClick={() => handleEditProduct(p.id, p.name, p.price)}
                                            style={{
                                                background: 'none',
                                                border: 'none',
                                                cursor: 'pointer',
                                                fontSize: '1.2em',
                                                color: colors.pink
                                            }}
                                        >
                                            <GrEdit />
                                        </button>

                                        <button
                                            onClick={() => handleDeleteProduct(p.id)}
                                            style={{
                                                background: 'none',
                                                border: 'none',
                                                cursor: 'pointer',
                                                fontSize: '1.2em',
                                                color: colors.pink
                                            }}
                                        >
                                            <FaTrash />
                                        </button>
                                    </div>


                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}

            {/* -- TAB: OPEN ORDERS -- */}
            {activeTab === 'open' && (
                <div>
                    <div style={{ ...styles.card, borderLeftColor: colors.pink }}>
                        <h3>🛍️ Create New Order</h3>
                        <input style={styles.input} placeholder="Customer Full Name" value={customerName} onChange={e => setCustomerName(e.target.value)} />

                        <div style={{ display: 'flex', gap: '10px', alignItems: 'center' }}>
                            <select style={{ ...styles.input, flex: 1, marginBottom: 0 }} value={selectedProductId} onChange={e => setSelectedProductId(e.target.value)}>
                                <option value="">Select a dessert...</option>
                                {products.map(p => <option key={p.id} value={p.id}>{p.name} ($ {p.price?.toFixed(2)})</option>)}
                            </select>
                            <input type="number" style={{ ...styles.input, width: '70px', marginBottom: 0 }} value={quantity} onChange={e => setQuantity(e.target.value)} min="1" />
                            <button onClick={addToTempOrder} style={{ ...styles.tabBtn(true, colors.brown), padding: '10px 15px', fontSize: '14px', marginBottom: 0 }}>Add</button>
                        </div>

                        {tempItems.length > 0 && (
                            <ul style={{ ...styles.productList, backgroundColor: `${colors.bg}44`, padding: '15px', borderRadius: '8px', marginTop: '15px' }}>
                                {tempItems.map((it, i) => <li key={i}>{it.quantity}x {it.name} - R$ {(it.price * it.quantity).toFixed(2)}</li>)}
                            </ul>
                        )}

                        <button onClick={handleCreateOrder} style={{ ...styles.btnMain, marginTop: '20px' }}>Confirm Order</button>
                    </div>

                    <h3>Pending Orders ({openOrders.length})</h3>
                    {openOrders.map(o => <OrderCard order={o} showActions={true} statusColor={colors.pink} statusText="OPEN" />)}
                </div>
            )}

            {/* --- ABA: ENTREGUES --- */}
            {activeTab === 'finalized' && (
                <div>
                    <h3>History of Delights Delivered ({finalizedOrders.length})</h3>
                    {finalizedOrders.map(o => <OrderCard order={o} showActions={false} statusColor="#2196f3" statusText="DELIVERED" />)}
                </div>
            )}

            {/* --- ABA: CANCELADAS --- */}

            {activeTab === 'cancelled' && (
                <div>
                    <h3>Cancelled Orders ({cancelledOrders.length})</h3>
                    {cancelledOrders.map(o => <OrderCard order={o} showActions={false} statusColor={colors.cancelled} statusText="CANCELED" />)}
                </div>
            )}
        </div>
    );
}