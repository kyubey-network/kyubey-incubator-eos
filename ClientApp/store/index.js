import Vue from 'vue'
import Vuex from 'vuex'
import { debug } from 'util';

Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
  counter: 1
}

const loginStateModule = {
  namespaced: true,
  state: {
    eosLoginState: {}
  },
  mutations: {},
  actions: {
    eosLogin: function (context, { account, loginMode, eos, requiredFields }) {
      debugger;
      context.state.eosLoginState = {
        account,
        loginMode,
        eos,
        requiredFields
      }
    }
  },
  getters: {}
}

// MUTATIONS
const mutations = {
  [MAIN_SET_COUNTER](state, obj) {
    state.counter = obj.counter
  }
}

// ACTIONS
const actions = ({
  setCounter({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj)
  }
})

export default new Vuex.Store({
  state,
  mutations,
  actions,
  modules: {
    loginState: loginStateModule
  }
})
