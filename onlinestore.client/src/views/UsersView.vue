<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <UserEditWindow v-if="isOpenDialog" @close-dialog="clickWindowRedactor" :user="selectedUser" />

  <div class="container">
    <h1 class="line">Пользователи</h1>
    <div class="catalog">
      <div class="user" v-for="(user, index) in users" :key="index" @click="clickOnItem(index)">
        <UserInfoBlock :modelValue="user.customer?.id" labelName="ID Заказчика:" :isGuid="user.customer?.id !== undefined" />
        <UserInfoBlock :modelValue="user.username" labelName="Никнейм:" :isGuid="false" />
        <UserInfoBlock :modelValue="user.role == 1 ? 'Менеджер' : 'Заказчик'" labelName="Роль:" :isGuid="false" />
        <UserInfoBlock :modelValue="user.customer?.name" labelName="Полное имя:" :isGuid="false" />
        <UserInfoBlock :modelValue="user.customer?.code" labelName="Код заказчика:" :isGuid="false" />
        <UserInfoBlock :modelValue="user.customer?.address" labelName="Адрес:" :isGuid="false" />
        <UserInfoBlock :modelValue="user.customer?.discount" labelName="Персональная скидка:" :isGuid="false" />
      </div>
    </div>

    <Pagination @page-changed="getUsers" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<script>
  import { getPageOfUsers } from '../api/usersApi';

  import Pagination from '../components/pagination/PaginationComponent.vue'
  import UserInfoBlock from '../components/blocks/UserInfoBlock.vue';
  import UserEditWindow from '../components/dialogs/UserEditWindow.vue';

  export default {
    name: 'Users',
    components: { Pagination, UserInfoBlock, UserEditWindow },
    data() {
      return {
        currentPage: 1,
        totalPages: 1,
        pageSize: 24,
        totalCount: 0,
        users: [],
        selectedUser: null,
        isOpenDialog: false
      }
    },
    methods: {
      async getUsers(number = 1) {
        this.currentPage = number;
        try {
          const response = await getPageOfUsers(this.currentPage, this.pageSize);
          if (response) {
            this.users = response.userResponses;
            this.totalCount = response.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Ошибка во время получения списка пользователей.');
          }
        } catch (error) {
          this.warnInfo('Ошибка при получении данных (Users): ', error);
        }
      },
      async clickOnItem(index) {
        this.selectedUser = this.users[index];
        await this.clickWindowRedactor(true);
      },
      async clickWindowRedactor(state) {
        this.isOpenDialog = state;
        if (!state) {
          await this.getUsers();
        }
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert(error.message);
      }
    },
    mounted() {
      this.getUsers();
    }
  }
</script>

<style scoped>
  .container {
    max-width: 65vw;
    margin: 0px auto;
    padding-top: 20px;
    background-color: #e0ebf7;
    filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
  }

  .catalog {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-evenly;
  }

  .user {
    align-items: center;
    box-shadow: 1px 2px 4px rgba(0, 50, 100, 0.2);
    align-content: space-around;
    margin: 10px;
    border-radius: 20px 20px 20px 20px;
    background-color: #f0f5fa;
    min-width: 300px;
    max-width: 400px;
  }

  .user:hover {
    box-shadow: 4px 8px 16px rgba(0, 50, 100, 0.4);
  }

  h1 {
    font-size: 26px;
    line-height: 36px;
    padding: 20px 10px;
    font-weight: 500;
    color: #1c2633;
  }

  .line {
    background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
  }
</style>
