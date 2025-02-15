<template>
    <div class="user-desc">
        <label>{{ labelName }}</label>
        <div v-if="isGuid" @click="copyText()" title="Копировать GUID">{{ modelValue ?? 'Нет' }}</div>
        <div v-else>{{ modelValue ?? 'Нет' }}</div>
    </div>
</template>

<script>
export default {
    props: {
        labelName: {
            type: String,
            required: true
        },
        modelValue: {},
        isGuid: {
            type: Boolean,
            required: true
        }
    },
    methods: {
        async copyText() {
            try {
                await navigator.clipboard.writeText(this.modelValue);
            } catch (error) {
                this.warnInfo('Скопировать GUID не удалось. ', error);
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert(error.message);
        }
    }
}
</script>

<style scoped>
    div {
        font-weight: 500;
        color: #1c2633;
    }

    label {
        font-weight: 500;
        color: #9199a2;
    }

    .user-desc {
        padding: 10px;
    }
</style>