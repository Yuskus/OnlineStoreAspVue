import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Users/getall?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Users/login [body]
// http://localhost:5000/api/Users/registercustomer [body]
// http://localhost:5000/api/Users/registermanager [body]
// http://localhost:5000/api/users/update/{this.user.customerId} [body]
// http://localhost:5000/api/users/delete/${this.user.customerId}

export const getPageOfUsers = (pageNumber, pageSize) => {

}

export const logIn = (logForm) => {

}

export const registerCustomer = (customer) => {

}

export const registerManager = (manager) => {

}

export const updateUser = (userId, newUser) => {

}

export const deleteUser = (userId) => {
    
}