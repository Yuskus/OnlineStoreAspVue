<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>{{ windowName }}</h2>
            <div v-if="isCreate">
                <RegistrationForm v-model="localUser" :isCustomer="isCustomer"/>
            </div>
            <div v-else>
                <DialogLineForm v-model="localUser.username" labelName="Никнейм" inputType="text" placeholderText="user1234" />
                <DialogLineForm v-model="localUser.role" labelName="Роль" inputType="number" placeholderText="Роль пользователя" />
                <div v-if="localUser.customer">
                    <DialogLineForm v-model="localUser.customer.id" labelName="ID Заказчика" inputType="text" placeholderText="6F9619FF-8B86-D011-B42D-00CF4FC964FF" />
                    <DialogLineForm v-model="localUser.customer.name" labelName="Имя" inputType="text" placeholderText="Имя заказчика" />
                    <DialogLineForm v-model="localUser.customer.code" labelName="Код" inputType="text" placeholderText="1234-2000" />
                    <DialogLineForm v-model="localUser.customer.address" labelName="Адрес" inputType="text" placeholderText="Адрес заказчика" />
                    <DialogLineForm v-model="localUser.customer.discount" labelName="Скидка" inputType="number" placeholderText="Процент скидки (целое число)" />
                </div>
            </div>
            <div class="buttons">
                <button v-show="isCreate" @click="changeRole()">Переключить на {{ isCustomer ? 'менеджера' : 'заказчика' }}</button>
                <button v-show="isCreate" @click="addUser()">Добавить {{ isCustomer ? 'заказчика' : 'менеджера' }}</button>
                <button v-show="!isCreate" @click="applyChanges()">Изменить</button>
                <button v-show="!isCreate" @click="deleteUser()">Удалить</button>
                <button @click="cancelDialog()">Назад</button>
            </div>
        </div>
    </div>
</template>

<script>
import { updateUser, deleteUser, registerCustomer, registerManager } from '../../api/usersApi';
import { updateCustomer } from '../../api/customersApi';

import DialogLineForm from '../forms/DialogLineForm.vue';
import RegistrationForm from '../forms/RegistrationForm.vue';

export default {
    components: { DialogLineForm, RegistrationForm },
    props: {
        user: {
            type: Object,
            required: true
        },
        isCreate: {
            type: Boolean,
            default: false
        }
    },
    data() {
        return {
            windowName: this.isCreate ? 'Добавить пользователя' : 'Редактировать пользователя',
            localUser: { ...this.user },
            isCustomer: true
        }
    },
    methods: {
        changeRole() {
            this.isCustomer = !this.isCustomer;
        },
        async addUser() {
            try {
                const response = this.isCustomer ? await registerCustomer(this.localUser) : await registerManager(this.localUser);
                if (!response) {
                    alert('Ошибка при добавлении пользователя.');
                }
            } catch (error) {
                this.warnInfo('Ошибка при добавлении данных (UserEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async applyChanges() {
            try {
                if (this.localUser.customer) {
                    const responseCustomer = await updateCustomer(this.localUser.customer.id, this.localUser.customer);
                    if (!responseCustomer) {
                        alert('Ошибка при изменении пользователя (Customer part).');
                    }
                }
                const responseUser = await updateUser(this.localUser.username, {
                    username: this.localUser.username,
                    role: parseInt(this.localUser.role)
                });
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
    max-height: 65vh;
    overflow-y: scroll;
}

h2 {
    color: #212933;
    padding-bottom: 20px;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
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

button:active {
    background-color: rgba(0,0,30,0.4);
}

::-webkit-scrollbar {
    width: 12px;
    height: 12px;
    border-radius: 5px;
}

::-webkit-scrollbar-thumb {
    background-color: #9098a3;
    border-radius: 5px;
}

::-webkit-scrollbar-track {
    background: white;
    border-radius: 0px 5px 5px 0px;
}
</style>