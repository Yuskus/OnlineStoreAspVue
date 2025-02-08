import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfOrders = async (pageNumber, pageSize) => {
    try {
        if (!checkPages(pageNumber, pageSize)) throw new Error("Ошибка валидации страниц: pageNumber, pageSize (Order).");

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
        if (!checkGuid(customerId)) throw new Error("Ошибка валидации Guid (Order).");
        if (!checkPages(pageNumber, pageSize)) throw new Error("Ошибка валидации страниц: pageNumber, pageSize (Order).");

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
        if (!checkGuid(customerId)) throw new Error("Ошибка валидации Guid (Order).");

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
        if (!checkGuid(orderId)) throw new Error("Ошибка валидации Guid (Order).");

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
        if (!checkGuid(orderId)) throw new Error("Ошибка валидации Guid (Order).");
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
        if (!checkGuid(orderId)) throw new Error("Ошибка валидации Guid (Order).");

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
    let validationErrors = [];

    if (!checkGuid(order.customerId)) validationErrors.push("Ошибка валидации customerId (order).");
    if (!checkOrderDate(order.orderDate)) validationErrors.push("Ошибка валидации orderDate (order).");
    if (!checkShipmentDate(order.shipmentDate)) validationErrors.push("Ошибка валидации shipmentDate (order).");
    if (!checkOrderStatus(order.orderStatus)) validationErrors.push("Ошибка валидации orderStatus (order).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации Order");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const checkGuid = (id) => {
    return id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
}

const checkPages = (pageNumber, pageSize) => {
    return Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 50;
}

const checkOrderDate = (orderDate) => {
    return !isNaN(Date.parse(orderDate));
}

const checkShipmentDate = (shipmentDate) => {
    return shipmentDate === null || !isNaN(Date.parse(shipmentDate));
}

const checkOrderStatus = (status) => {
    return status === 'new' || status === 'in progress' || status === 'completed';
}