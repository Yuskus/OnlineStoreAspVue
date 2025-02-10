import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getOrderElementByOrderId = async (orderId) => {
    try {
        validateGuid(orderId);

        const response = await axios.get(`${API_URL}/api/orderelements/getbyorderid/${orderId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getOrderElementByOrderId): ', error);
        throw error;
    }
}

export const addOrderElement = async (orderElement) => {
    try {
        validateOrderElementRequest(orderElement);

        const response = await axios.post(`${API_URL}/api/orderelements/add`, orderElement, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при добавлении данных (addOrderElement): ', error);
        throw error;
    }
}

export const updateOrderElement = async (orderElementId, newOrderElement) => {
    try {
        validateGuid(orderElementId);
        validateOrderElementRequest(newOrderElement);

        const response = await axios.put(`${API_URL}/api/orderelements/update/${orderElementId}`, newOrderElement, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateOrderElement): ', error);
        throw error;
    }
}

export const deleteOrderElement = async (orderElementId) => {
    try {
        validateGuid(orderElementId);

        const response = await axios.delete(`${API_URL}/api/orderelements/delete/${orderElementId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteOrderElement): ', error);
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

const validateOrderElementRequest = (orderElement) => {
    validateGuid(orderElement.orderId);
    validateGuid(orderElement.itemId);
    validateCount(orderElement.itemsCount);
    validatePrice(orderElement.itemPrice);
}

const validateGuid = (id) => {
    const isValid = id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
    if (!isValid) throw new Error('Ошибка валидации guid.');
}

const validateCount = (count) => {
    const isValid = count % 1 === 0 && count > 0;
    if (!isValid) throw new Error('Ошибка валидации count.');
}

const validatePrice = (price) => {
    const isValid = !isNaN(price) && price > 0;
    if (!isValid) throw new Error('Ошибка валидации price.');
}