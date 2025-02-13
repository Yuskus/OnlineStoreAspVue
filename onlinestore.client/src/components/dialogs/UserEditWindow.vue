<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать пользователя</h2>

            <DialogLineForm v-model="localUser.username" :labelName="'Никнейм'" :inputText="localUser.username" />
            <DialogLineForm v-model="localUser.role" :labelName="'Роль'" :inputType="'number'" :inputText="localUser.role" />

            <div v-if="localUser.customer">
                <DialogLineForm v-model="localUser.customer.id" :labelName="'ID Заказчика'" :inputText="localUser.customer.id" />
                <DialogLineForm v-model="localUser.customer.name" :labelName="'Имя'" :inputText="localUser.customer.name" />
                <DialogLineForm v-model="localUser.customer.code" :labelName="'Код'" :inputText="localUser.customer.code" />
                <DialogLineForm v-model="localUser.customer.address" :labelName="'Адрес'" :inputText="localUser.customer.address" />
                <DialogLineForm v-model="localUser.customer.discount" :labelName="'Скидка'" :inputType="'number'" :inputText="localUser.customer.discount" />
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
import { updateUser, deleteUser } from '../../api/usersApi';
import { updateCustomer } from '../../api/customersApi';

import DialogLineForm from '../forms/DialogLineForm.vue';

export default {
    components: { DialogLineForm },
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
                role: parseInt(this.localUser.role)
            };
        },
        async applyChanges() {
            try {
                if (this.localUser.customer) {
                    let newCustomer = this.makeCustomerRequestBody();
                    const responseCustomer = await updateCustomer(this.localUser.customer.id, newCustomer);
                    if (!responseCustomer) {
                        alert('Ошибка при изменении пользователя (Customer part).');
                    }
                }
                let newUser = this.makeUserRequestBody();
                const responseUser = await updateUser(this.localUser.username, newUser);
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
                const response = await deleteUser(this.localUser.username);
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
            alert(error.message);
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

h2 {
    color: #212933;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

button:hover {
    background-color: rgba(0,0,30,0.25);
}

button:focus {
    background-color: rgba(0,0,30,0.4);
}
</style>