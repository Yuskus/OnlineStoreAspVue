import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfOrders = async (pageNumber, pageSize) => {
    try {
        validatePages(pageNumber, pageSize);

        const response = await axios.get(`${API_URL}/api/orders/getpage?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfOrders): ', error);
        throw error;
    }
}

export const getPageOfOrdersByCustomer = async (customerId, pageNumber, pageSize) => {
    try {
        validateGuid(customerId);
        validatePages(pageNumber, pageSize);

        const response = await axios.get(`${API_URL}/api/orders/getpagebycustomer/${customerId}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });
        
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfOrdersByCustomer): ', error);
        throw error;
    }
}

export const getBasket = async (customerId) => {
    try {
        validateGuid(customerId);

        const response = await axios.get(`${API_URL}/api/orders/getbasket/${customerId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });
        
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getBasket): ', error);
        throw error;
    }
}

export const placeAnOrder = async (orderId) => {
    try {
        validateGuid(orderId);

        const response = await axios.patch(`${API_URL}/api/orders/placeanorder/${orderId}`, {}, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });
        
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при оформлении заказа (placeAnOrder): ', error);
        throw error;
    }
}

export const updateOrder = async (orderId, newOrder) => {
    try {
        validateGuid(orderId);
        validateOrderRequest(newOrder);

        const response = await axios.put(`${API_URL}/api/orders/update/${orderId}`, newOrder, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });
        
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateOrder): ', error);
        throw error;
    }
}

export const deleteOrder = async (orderId) => {
    try {
        validateGuid(orderId);

        const response = await axios.delete(`${API_URL}/api/orders/delete/${orderId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteOrder): ', error);
        throw error;
    }
}

const handleResponse = (response) => {
    if (response.status === 200 && response.data) {
        return response.data;
    } else {
        console.log("Статус операции: " + response.status + ".");
        return null;
    }
};

const validateOrderRequest = (order) => {
    validateGuid(order.customerId);
    validateOrderDate(order.orderDate);
    validateShipmentDate(order.shipmentDate);
    validateOrderStatus(order.orderStatus);
}

const validateGuid = (id) => {
    const isValid = id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
    if (!isValid) throw new Error('Ошибка валидации guid.');
}

const validatePages = (pageNumber, pageSize) => {
    const isValid = Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 50;
    if (!isValid) throw new Error('Ошибка валидации pageNumber, pageSize.');
}

const validateOrderDate = (orderDate) => {
    const isValid = !isNaN(Date.parse(orderDate));
    if (!isValid) throw new Error('Ошибка валидации orderDate.');
}

const validateShipmentDate = (shipmentDate) => {
    const isValid = shipmentDate === null || !isNaN(Date.parse(shipmentDate));
    if (!isValid) throw new Error('Ошибка валидации shipmentDate.');
}

const validateOrderStatus = (status) => {
    const isValid = status === 'new' || status === 'in progress' || status === 'completed';
    if (!isValid) throw new Error('Ошибка валидации status.');
}