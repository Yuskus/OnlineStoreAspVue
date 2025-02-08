import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/OrderElements/getbyid/${this.basketOrder.id}
// http://localhost:5000/api/OrderElements/add [body]
// http://localhost:5000/api/orderelements/update/${this.localItem.id} [body]
// http://localhost:5000/api/orderelements/delete/${this.localItem.id}

export const getOrderElementByOrderId = async (orderId) => {
    try {
        if (!validateGuid(orderId)) throw new Error("Ошибка валидации Guid.");

        const response = await axios.get(`${API_URL}/api/orderelements/getbyorderid/${orderId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getOrderElementByOrderId): ', error);
        alert('Проблемы с сервером!');
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
        alert('Проблемы с сервером!');
    }
}

export const updateOrderElement = async (orderElementId, newOrderElement) => {
    try {
        if (!validateGuid(orderElementId)) throw new Error("Ошибка валидации Guid.");
        validateOrderElementRequest(newOrderElement);

        const response = await axios.put(`${API_URL}/api/orderelements/update/${orderElementId}`, newOrderElement, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateOrderElement): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteOrderElement = async (orderElementId) => {
    try {
        if (!validateGuid(orderElementId)) throw new Error("Ошибка валидации Guid.");

        const response = await axios.delete(`${API_URL}/api/orderelements/delete/${orderElementId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteOrderElement): ', error);
        alert('Проблемы с сервером!');
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
    let validationErrors = [];

    if (!validateGuid(orderElement.orderId)) validationErrors.push("Ошибка валидации orderId (orderElement).");
    if (!validateGuid(orderElement.itemId)) validationErrors.push("Ошибка валидации itemId (orderElement).");
    if (!validateCount(orderElement.itemsCount)) validationErrors.push("Ошибка валидации itemsCount (orderElement).");
    if (!validatePrice(orderElement.itemPrice)) validationErrors.push("Ошибка валидации itemPrice (orderElement).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации OrderElement");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const validateGuid = (id) => {
    return id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
}

const validateCount = (count) => {
    return Number.isInteger(count) && count > 0;
}

const validatePrice = (price) => {
    return !isNaN(price) && price > 0;
}