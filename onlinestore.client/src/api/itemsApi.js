import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfItems = async (pageNumber, pageSize) => {
    try {
        if (!validatePages(pageNumber, pageSize)) throw new Error("Ошибка валидации страниц: pageNumber, pageSize (Item).");

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
        if (!validateCategory(category)) throw new Error("Ошибка валидации category (Item).");
        if (!validatePages(pageNumber, pageSize)) throw new Error("Ошибка валидации pageNumber, pageSize (Item).");

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
        if (!validateGuid(itemId)) throw new Error("Ошибка валидации Guid.");
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
        if (!validateGuid(itemId)) throw new Error("Ошибка валидации Guid.");

        const response = await axios.put(`${API_URL}/api/items/delete/${itemId}`, {
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
    let validationErrors = [];

    if (!validateCode(item.code)) validationErrors.push("Ошибка валидации code (Item).");
    if (!validateName(item.name)) validationErrors.push("Ошибка валидации name (Item).");
    if (!validatePrice(item.price)) validationErrors.push("Ошибка валидации price (Item).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации Item");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const validateGuid = (id) => {
    return id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
}

const validatePages = (pageNumber, pageSize) => {
    return Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 36;
}

const validateCode = (code) => {
    return code && code.length === 12 && /^[0-9]{2}-[0-9]{4}-[A-Z]{2}[0-9]{2}$/.test(code);
}

const validateName = (name) => {
    return name && name.trim() > 0;
}

const validatePrice = (price) => {
    return !isNaN(price) && price > 0;
}

const validateCategory = (category) => {
    return category && category.trim() > 0;
}