import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'

Vue.use(Vuex)

const vuexLocal = new VuexPersistence({
  key: process.env.VUE_APP_STORE_KEY,
  storage: window.sessionStorage
})

export default new Vuex.Store({
  state: {
    characterCreation: {
      step1: null,
      step2: null
    },
    locale: null,
    token: null,
    world: null
  },
  actions: {
    changeWorld({ commit }, world) {
      commit('setWorld', world)
    },
    resetCharacterCreation({ commit }) {
      commit('setCharacterCreationStep1', null)
      commit('setCharacterCreationStep2', null)
    },
    resetWorld({ commit }) {
      commit('setWorld', null)
    },
    saveCharacterCreationStep1({ commit }, payload) {
      commit('setCharacterCreationStep1', payload)
    },
    saveCharacterCreationStep2({ commit }, payload) {
      commit('setCharacterCreationStep2', payload)
    },
    signIn({ commit }, token) {
      commit('setToken', token)
    },
    signOut({ commit }) {
      commit('setToken', null)
      commit('setWorld', null)
    },
    translate({ commit }, locale) {
      commit('setLocale', locale)
    }
  },
  mutations: {
    setCharacterCreationStep1(state, payload) {
      state.characterCreation.step1 = payload
    },
    setCharacterCreationStep2(state, payload) {
      state.characterCreation.step2 = payload
    },
    setLocale(state, locale) {
      state.locale = locale
    },
    setToken(state, token) {
      state.token = token
    },
    setWorld(state, world) {
      state.world = world
    }
  },
  plugins: [vuexLocal.plugin]
})
