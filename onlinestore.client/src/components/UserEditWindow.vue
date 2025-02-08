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

<script>
import usersApi from '../api/usersApi';
import customersApi from '../api/customersApi';

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
        makeCustomerRequestBody() {
            return {
                name: this.localUser.customer.name,
                code: this.localUser.customer.code,
                address: this.localUser.customer.address,
                discount: this.localUser.customer.discount
            };
        },
        makeUserRequestBody() {
            return {
                username: this.localUser.username,
                role: this.localUser.role
            };
        },
        async applyChanges() {
            try {
                let newCustomer = this.makeCustomerRequestBody();
                let newUser = this.makeUserRequestBody();

                const responseCustomer = await customersApi.updateCustomer(this.localUser.customerId, newCustomer);
                const responseUser = await usersApi.updateUser(this.localUser.username, newUser);

                if (!responseCustomer) {
                    alert('Ошибка при изменении пользователя (Customer part).');
                }
                if (!responseUser) {
                    alert('Ошибка при изменении пользователя (User part).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при изменении данных (UserEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async deleteUser() {
            try {
                const response = await usersApi.deleteUser(this.localUser.username);
                if (!response) {
                    alert('Ошибка при удалении пользователя.');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (UserEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert('Проблемы с сервером!');
        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    }
}
</script>

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