import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfItems = async (pageNumber, pageSize) => {
    try {
        validatePages(pageNumber, pageSize);

        const response = await axios.get(`${API_URL}/api/items/getpage?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfItems): ', error);
        throw error;
    }
}

export const getPageOfItemsByCategory = async (category, pageNumber, pageSize) => {
    try {
        validateCategory(category);
        validatePages(pageNumber, pageSize);

        const response = await axios.get(`${API_URL}/api/items/getpagebycategory/${category}?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfItemsByCategory): ', error);
        throw error;
    }
}

export const getCategories = async () => {
    try {
        const response = await axios.get(`${API_URL}/api/items/getcategories`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getCategories): ', error);
        throw error;
    }
}

export const addItem = async (item) => {
    try {
        validateItemRequest(item);

        const response = await axios.post(`${API_URL}/api/items/add`, item, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при добавлении данных (addItem): ', error);
        throw error;
    }
}

export const updateItem = async (itemId, newItem) => {
    try {
        validateGuid(itemId);
        validateItemRequest(newItem);

        const response = await axios.put(`${API_URL}/api/items/update/${itemId}`, newItem, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateItem): ', error);
        throw error;
    }
}

export const deleteItem = async (itemId) => {
    try {
        validateGuid(itemId);

        const response = await axios.delete(`${API_URL}/api/items/delete/${itemId}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteItem): ', error);
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

const validateItemRequest = (item) => {
    validateCode(item.code);
    validateName(item.name);
    validatePrice(item.price);
}

const validateGuid = (id) => {
    const isValid = id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
    if (!isValid) throw new Error('Ошибка валидации guid.');
}

const validatePages = (pageNumber, pageSize) => {
    const isValid = Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 36;
    if (!isValid) throw new Error('Ошибка валидации pageNumber, pageSize.');
}

const validateCode = (code) => {
    const isValid = code && code.length === 12 && /^[0-9]{2}-[0-9]{4}-[A-Z]{2}[0-9]{2}$/.test(code);
    if (!isValid) throw new Error('Ошибка валидации code.');
}

const validateName = (name) => {
    const isValid = name && name.trim().length > 0;
    if (!isValid) throw new Error('Ошибка валидации name.');
}

const validatePrice = (price) => {
    const isValid = !isNaN(price) && price > 0;
    if (!isValid) throw new Error('Ошибка валидации price.');
}

const validateCategory = (category) => {
    const isValid = category && category.trim().length > 0;
    if (!isValid) throw new Error('Ошибка валидации category.');
}