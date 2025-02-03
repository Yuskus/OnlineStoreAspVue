<script>
import axios from 'axios';

export default {
    props: {
        user: {
            type: Object
        }
    },
    data() {
        return {
            localUser: { ...this.user }
        }
    },
    methods: {
        async deleteUser() {
            let url;
            if (this.user.customer.id !== null) {
                url = `http://localhost:5000/api/customers/delete/${this.user.customerId}`;
            } else {
                url = `http://localhost:5000/api/users/delete/${this.user.customerId}`;
            }
            try {
                const response = await axios.delete(url, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response.status === 200 && response.data) {
                    console.log("Удалено");
                } else {
                    alert('Проблемы с сервером!');
                    console.log("Статус ошибки: " + response.status);
                }
                this.cancelDialog();
            } catch (error) {
                console.error('Ошибка при получении данных (UserEdit): ', error);
                alert('Проблемы с сервером!');
                this.cancelDialog();
            }
        },
        async applyChanges() {
            try {
                let a = {
                    customerId: this.localUser.customerId,
                    username: this.localUser.username,
                    password: "xxxxxxxxx",
                    role: this.localUser.role
                };
                let b = {
                    name: this.localUser.customer.name,
                    code: this.localUser.customer.code,
                    address: this.localUser.customer.address,
                    discount: this.localUser.customer.discount
                };
                console.log(a);
                console.log(b);
                const response = await axios.put(`http://localhost:5000/api/users/update/`, {
                    customerId: this.localUser.customerId,
                    username: this.localUser.username,
                    password: "xxxxxxxxx",
                    role: this.localUser.role
                }, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });

                if (response.status === 200 && response.data) {
                    console.log("Обновлено");
                } else {
                    alert('Проблемы с сервером!');
                    console.log("Статус ошибки: " + response.status);
                }

                const response2 = await axios.put(`http://localhost:5000/api/customers/update/${this.localUser.customerId}`, {
                    name: this.localUser.customer.name,
                    code: this.localUser.customer.code,
                    address: this.localUser.customer.address,
                    discount: this.localUser.customer.discount
                }, {
                    headers: {
                        'authorization': `Bearer ${localStorage.getItem('jwt')}`
                    }
                });
          
                if (response2.status === 200 && response2.data) {
                    console.log("Обновлено");
                } else {
                    alert('Проблемы с сервером!');
                    console.log("Статус ошибки: " + response2.status);
                }
                this.cancelDialog();
            } catch (error) {
                console.error('Ошибка при получении данных (UserEdit): ', error);
                alert('Проблемы с сервером!');
                this.cancelDialog();
            }
        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    }
}
</script>
<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать пользователя</h2>
            <hr>
            <div class="form">
                <label>ID Заказчика</label>
                <input type="text" v-model="localUser.customerId">
            </div>
            <hr>
            <div class="form">
                <label>Никнейм</label>
                <input type="text" v-model="localUser.username">
            </div>
            <hr>
            <div class="form">
                <label>Роль</label>
                <input type="number" v-model="localUser.role">
            </div>
            <hr>
            <div class="form">
                <label>Имя</label>
                <input type="text" v-model="localUser.customer.name">
            </div>
            <hr>
            <div class="form">
                <label>Код</label>
                <input type="text" v-model="localUser.customer.code">
            </div>
            <hr>
            <div class="form">
                <label>Адрес</label>
                <input type="text" v-model="localUser.customer.address">
            </div>
            <hr>
            <div class="form">
                <label>Скидка</label>
                <input type="number" v-model="localUser.customer.discount">
            </div>
    
            <div class="buttons">
                <button @click="applyChanges()">Изменить</button>
                <button @click="deleteUser()">Удалить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
</template>
<style scoped>
.dialog-over {
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 0px;
    bottom: 0px;
    left: 0px;
    right: 0px;
    transform: scale(1.25);
    background-color: rgba(0,0,0,0.4);
    z-index: 1000;
}

.dialog {
    background-color: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
    min-width: 400px;
    width: 50vw;
    min-height: fit-content;
}

.form label {
    width: 50%;
    margin-right: 10px;
    padding: 10px;
}

.form input {
    width: 50%;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid rgba(0,0,80,0.5);
}

h2 {
    color: #212933;
}

h2, label, button {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-style: normal;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

.buttons button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

.buttons button:hover {
    background-color: rgba(0,0,30,0.25);
}

.buttons button:focus {
    background-color: rgba(0,0,30,0.4);
}

.form {
    display: flex;
    margin-bottom: 15px;
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>